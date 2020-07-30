using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Chris.Personnel.Management.ViewModel;

namespace Chris.Personnel.Management.Blazor.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<UserFormViewModel>> GetUsersAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<UserFormViewModel>>(
                await _httpClient.GetStreamAsync("api/users"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}