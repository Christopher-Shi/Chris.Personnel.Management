﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BootstrapBlazor.Components;
using Chris.Personnel.Management.Blazor.Services;
using Chris.Personnel.Management.ViewModel;
using Microsoft.AspNetCore.Components;

namespace Chris.Personnel.Management.Blazor.Pages
{
    public partial class User
    {
        [Inject]
        public IUserService UserService { get; set; }

        public IEnumerable<UserFormViewModel> Users { get; set; } = new List<UserFormViewModel>();

        public UserFormViewModel SearchModel { get; set; } = new UserFormViewModel();

        protected override async Task OnInitializedAsync()
        {
            Users = await UserService.GetUsersAsync();

            await base.OnInitializedAsync();
        }
    }
}
