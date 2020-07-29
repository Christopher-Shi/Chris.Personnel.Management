using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Chris.Personnel.Management.ViewModel;
using Microsoft.AspNetCore.Components;

namespace Chris.Personnel.Management.Blazor.Pages
{
    public class UserBase : ComponentBase
    {
        [Inject]
        public HttpClient HttpClient { get; set; }

        public IEnumerable<UserFormViewModel> Users { get; set; } = new List<UserFormViewModel>();

        protected override async Task OnInitializedAsync()
        {
            Users = await HttpClient.GetFromJsonAsync<IEnumerable<UserFormViewModel>>("api/Users");

            await base.OnInitializedAsync();
        }
    }
}
