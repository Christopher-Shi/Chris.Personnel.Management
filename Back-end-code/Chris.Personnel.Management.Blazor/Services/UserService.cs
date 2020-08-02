using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common.Enums;
using Chris.Personnel.Management.UICommand;
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

        public async Task<UserPaginationViewModel> GetByPageAsync(string trueName,
            Gender? gender,
            IsEnabled? isEnabled,
            int current,
            int pageSize,
            string orderByPropertyName,
            bool isAsc)
        {
            //return await JsonSerializer.DeserializeAsync<UserPaginationViewModel>(
            //    await _httpClient.GetStreamAsync("api/users/pagination"),
            //    new JsonSerializerOptions
            //    {
            //        PropertyNameCaseInsensitive = true
            //    });

            return await JsonSerializer.DeserializeAsync<UserPaginationViewModel>(
                await _httpClient.GetStreamAsync("api/users/pagination"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task AddUser(UserAddUICommand user)
        {
            var employeeJson =
                new StringContent(
                    JsonSerializer.Serialize(user),
                    Encoding.UTF8,
                    "application/json");

            var response = await _httpClient.PostAsync(
                $"api/Users", employeeJson);

            //if (response.IsSuccessStatusCode)
            //{
            //    return await JsonSerializer.DeserializeAsync<UserAddUICommand>
            //        (await response.Content.ReadAsStreamAsync());
            //}

            //return null;
        }
    }
}