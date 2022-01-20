using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Incident.Comm.Integration.Api.Config;
using Incident.Comm.Integration.Api.Middlewares;
using Incident.Comm.Integration.Api.Services.Caching;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.IO;
using YorkshireWater.AppInsights;
using YorkshireWater.Common.Security.Headers;
using Incident.Comm.Integration.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection; 
using Incident.Comm.Integration.Data.Interfaces;
using Incident.Comm.Integration.Data.Repositiories;
using Incident.Comm.Integration.Api.Services;
using YorkshireWater.Common.Tokens.Jwt.IdentityServer;
using YorkshireWater.Common.Tokens.Jwt;
using Incident.Comm.Integration.Api.Identity;

namespace Incident.Comm.Integration.Api
{
    public class Startup
    {
        // Don't need a leading '/'
        private const string SwaggerRoutePrefix = "api/swagger";

        private readonly string _title = typeof(Startup).Assembly.GetName().Name;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register database context
            RegisterDbContext(services);

            services.AddCors();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddYorkshireWaterAppInsights();
            services.AddApplicationInsightsTelemetry();
            services.AddControllers();

            var errorsSection = Configuration.GetSection("Errors").Get<ErrorsSection>();
            services.AddSingleton(errorsSection);
            var idConfig = Configuration.GetSection("IdentityGateway").Get<IdentityGatewaySection>();
            services.AddSingleton(idConfig);
            var cacheConfig = Configuration.GetSection("Caching").Get<CachingSection>();
            services.AddSingleton(cacheConfig);
            var redisConfig = Configuration.GetSection("Redis").Get<RedisSection>();
            services.AddSingleton(redisConfig);
            var crossSiteSecurityConfig = Configuration.GetSection("CrossSiteSecurity").Get<CrossSiteSecuritySection>();
            services.AddSingleton(crossSiteSecurityConfig);

            services.AddTransient<ITokenClient, IdentityServerTokenClient>();
            services.AddTransient<IIdentityServerClient, IdentityServerClient>();

            AddCustomServices(services);

            if (cacheConfig.UseRedis)
            {
                services.AddStackExchangeRedisCache(option =>
                {
                    option.Configuration = redisConfig.Configuration;
                    option.InstanceName = redisConfig.InstanceName;
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }
            /*
            // Uncomment this section to make all controller methods authorised by default.

            services.AddMvc(config => {
                var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            */
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = idConfig.BaseUrl;
                    options.RequireHttpsMetadata = true;
                    options.ApiName = idConfig.ThisApiScope;
                });
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = _title,
                    Version = "v1",
                    Description = @"Operational Events Incident Comm Integration API"
                });

                // Added following sections to support bearer token within swagger doc
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. <br/> 
                          Enter 'Bearer' [space] and then your token in the text input below. <br/>
                          Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                Scheme = "oauth2",
                                Name = "Bearer",
                                In = ParameterLocation.Header
                            },
                            new List<string>()
                        }
                    });

                // Set the comments path for the Swagger JSON and UI.
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{_title}.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseSecurityHeaders(new SecurityHeadersBuilder().AddDefaultSecurePolicy());
            app.UseExceptionHandler("/Errors");

            if (env.IsDevelopment())
            {
                app.UseAppInsightsRequestResponseLogger();
            }

            app.UseSecurityHeaders(new SecurityHeadersBuilder().AddDefaultSecurePolicy());
            app.UseExceptionHandler("/Errors");

            if (!env.IsProduction())
            {
                if (SwaggerRoutePrefix == null)
                {
                    throw new NotImplementedException($"{nameof(SwaggerRoutePrefix)} must be set.");
                }

                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger(opt => { opt.RouteTemplate = SwaggerRoutePrefix + "/{documentName}/swagger.json"; });

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.RoutePrefix = SwaggerRoutePrefix;
                    c.SwaggerEndpoint($"/{SwaggerRoutePrefix}/v1/swagger.json", _title + " v1");
                    c.SupportedSubmitMethods(new[] { SubmitMethod.Get, SubmitMethod.Head, SubmitMethod.Delete, SubmitMethod.Post, SubmitMethod.Put });
                });
            }

            //app.UseHttpsRedirection(); //Comment this line out as code in AlwaysOnController is not working (suggestion by Eddie)
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            ConfigureCORS(app, env);

            // global error handler
            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
        }

        private static void ConfigureCORS(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            }
            if (env.IsStaging())
            {
                app.UseCors(builder =>
                    builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins("https://*.yorkswater.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            }
            if (env.IsProduction())
            {
                app.UseCors(builder =>
                    builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                    .WithOrigins("https://*.yorkshirewater.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            }
        }
        private void RegisterDbContext(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("incident-comm");
            var dbContextMigrationAssemblyName = typeof(IncidentCommDbContext).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<IncidentCommDbContext>(options =>
            {
                options.UseSqlServer(
                    connectionString,
                    m => m.MigrationsAssembly(dbContextMigrationAssemblyName));
            });
        }
        private static void AddCustomServices(IServiceCollection services)
        {
            services.AddTransient<IApiCache, ApiCache>();

            //Services
            services.AddTransient<IIncidentService, IncidentService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<INotificationTypeService, NotificationTypeService>();

            //Repositories
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
            services.AddScoped<IIncidentInfoRepository, IncidentInfoRepository>();
        }
    }
}
