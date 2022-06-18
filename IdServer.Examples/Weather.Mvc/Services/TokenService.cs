using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Options;

namespace Weather.Mvc.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly IdentityServerSettings _identityServerSettings;
        private readonly DiscoveryDocumentResponse _discoveryDocument;

        public TokenService(ILogger<TokenService> logger, IOptions<IdentityServerSettings> identityServerSettings)
        {
            _logger = logger;
            _identityServerSettings = identityServerSettings.Value;

            using var httpClient = new HttpClient();
            _discoveryDocument = httpClient.GetDiscoveryDocumentAsync(_identityServerSettings.DiscoveryUrl).Result;

            if (_discoveryDocument.IsError)
            {
                _logger.LogError($"Unable to get discovery document. Error is : {_discoveryDocument.Error}");
                throw new Exception("Unable to get discovery document", _discoveryDocument.Exception);
            }
        }
        public async Task<TokenResponse> GetToken(string scope)
        {
            using var client = new HttpClient();

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = _discoveryDocument.TokenEndpoint,
                ClientId = _identityServerSettings.ClientName,
                ClientSecret = _identityServerSettings.ClientPassword,
                Scope = scope
            });

            if (tokenResponse.IsError)
            {
                _logger.LogError($"Unable to get token. Error is : {tokenResponse.Error}");
                throw new Exception("Unable to get token", tokenResponse.Exception);
            }

            return tokenResponse;
        }
    }
}