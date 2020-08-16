using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common.DropDownListItems;
using Chris.Personnel.Management.ViewModel.DropDownListItems;

namespace Chris.Personnel.Management.Blazor.Services
{
    public class DropDownService : IDropDownService
    {
        private readonly HttpClient _httpClient;

        public DropDownService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<GenderDropDownListItem>> GetGenderEnumItemsAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<GenderDropDownListItem>>(
                await _httpClient.GetStreamAsync("api/dropDown/genders"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task<IEnumerable<IsEnabledDropDownListItem>> GetIsEnabledEnumItemsAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<IsEnabledDropDownListItem>>(
                await _httpClient.GetStreamAsync("api/dropDown/enabledStatus"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task<IEnumerable<RoleDropDownListViewModel>> GetRoleEnumItemsAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<RoleDropDownListViewModel>>(
                await _httpClient.GetStreamAsync("api/dropDown/roles"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }

        public async Task<IEnumerable<UserDropDownListViewModel>> GetUserEnumItemsAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<UserDropDownListViewModel>>(
                await _httpClient.GetStreamAsync("api/dropDown/users"),
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
        }
    }
}