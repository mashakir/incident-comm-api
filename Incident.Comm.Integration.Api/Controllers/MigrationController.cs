using Incident.Comm.Integration.Data;
using Incident.Comm.Integration.Data.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Incident.Comm.Integration.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class MigrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IncidentCommDbContext _dbContext;
        public MigrationController(IConfiguration configuration, IncidentCommDbContext dbContext)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }
        [HttpPost]
        [Produces(typeof(string))]
        public async Task<string> Post([FromBody] string migrationSecret)
        {
            var actualMigrationSecret = _configuration["MigrationSecret"];

            if (actualMigrationSecret == migrationSecret)
            {
                try
                {
                    if (!_dbContext.AllMigrationsApplied())
                    {
                        await _dbContext.Database.MigrateAsync();
                    }
                    return "Success";
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }

            return $"Not authorized: {migrationSecret} is not the secret.";
        }
    }
}
