using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Threading.Tasks;
using AntDesign;
using Chris.Personnel.Management.Blazor.Services;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.UICommand.DTO;
using Chris.Personnel.Management.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Chris.Personnel.Management.Blazor.Pages
{
    public partial class User
    {
        [Inject]
        public IUserService UserService { get; set; }

        [Inject]
        public ModalService ModalService { get; set; }

        public IEnumerable<UserFormViewModel> Users { get; set; } = new List<UserFormViewModel>();

        //public UserPaginationViewModel UsersByPage { get; set; } = new UserPaginationViewModel();

        public UserAddUICommand UserAddUICommand { get; set; } = new UserAddUICommand
        {
            User = new UserDTO()
        };

        public string txtValue { get; set; }

        private bool _editModalVisible;
        private bool _addModalVisible;

        private void ShowEditModal()
        {
            _editModalVisible = true;
        }

        private void HideEditModal()
        {
            _editModalVisible = false;
        }

        private void ShowAddModal()
        {
            _addModalVisible = true;
        }

        private async Task HideAddModal()
        {
            await UserService.AddUser(UserAddUICommand);
            _addModalVisible = false;
        }

        private async Task OnFinish()
        {
            await UserService.AddUser(UserAddUICommand);
        }

        private void OnFinishFailed(EditContext editContext)
        {
            Console.WriteLine($"Failed:{JsonSerializer.Serialize(UserAddUICommand)}");
        }

        protected override async Task OnInitializedAsync()
        {
            Users = await UserService.GetUsersAsync();

            await base.OnInitializedAsync();
        }
    }
}
