using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace Weather.Mvc.Services
{
    public interface ITokenService
    {
        Task<TokenResponse> GetToken(string scope);
    }
}