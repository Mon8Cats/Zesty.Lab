using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CloudCustomers.API.Config;
using CloudCustomers.API.Models;
using Microsoft.Extensions.Options;

namespace CloudCustomers.API.Services
{
    public class UsersService : IUsersService
    {
        private readonly HttpClient _httpClient;
        private readonly UsersApiOptions _apiOptions;
        public UsersService(HttpClient httpClient, IOptions<UsersApiOptions> apiConfig)
        {
            _httpClient = httpClient;
            _apiOptions = apiConfig.Value;

        }

        public async Task<List<User>> GetAllUsers()
        {
            var usersResponse = await _httpClient.GetAsync(_apiOptions.Endpoint);

            if (usersResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<User>();
            }

            var responseContent = usersResponse.Content;
            var allUsers = await responseContent.ReadFromJsonAsync<List<User>>();
            return allUsers.ToList();

        }
    }
}