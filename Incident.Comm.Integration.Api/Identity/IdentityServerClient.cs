using Incident.Comm.Integration.Api.Config;
using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using YorkshireWater.Common.Tokens.Jwt;

namespace Incident.Comm.Integration.Api.Identity
{
    public interface IIdentityServerClient
    {
        Task<string> GetTokenAsync(string identityScope);
    }

    public class IdentityServerClient : IIdentityServerClient
    {
        private readonly ITokenClient _tokenClient;
        private readonly ClientCredentials _credentials;
        private readonly string _baseUrl;

        public IdentityServerClient(ITokenClient tokenClient, IdentityGatewaySection identityGatewaySection)
        {
            _tokenClient = tokenClient ?? throw new ArgumentNullException(nameof(tokenClient));
            _baseUrl = identityGatewaySection.BaseUrl;
            _credentials = GetCredentials(identityGatewaySection.ClientId, identityGatewaySection.ClientSecret);
        }

        public Task<string> GetTokenAsync(string identityScope)
        {
            if (string.IsNullOrEmpty(identityScope)) throw new ArgumentNullException(nameof(identityScope));

            return GetTokenInternalAsync(identityScope);
        }

        private async Task<string> GetTokenInternalAsync(string identityScope)
        {
            var tokenResponse = await _tokenClient.GetTokenAsync(_baseUrl, _credentials, identityScope);

            if (!tokenResponse.IsSuccess)
            {
                throw new AuthenticationException("Identity server credentials were rejected");
            }

            return tokenResponse.AccessToken;
        }

        private static ClientCredentials GetCredentials(string clientId, string clientSecret)
        {
            return new ClientCredentials(clientId, clientSecret);
        }
    }
}
