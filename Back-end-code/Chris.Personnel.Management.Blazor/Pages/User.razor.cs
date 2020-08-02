using System.Collections.Generic;
using System.Threading.Tasks;
using AntDesign;
using Chris.Personnel.Management.Blazor.Services;
using Chris.Personnel.Management.ViewModel;
using Microsoft.AspNetCore.Components;

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

        private void HideAddModal()
        {
            _addModalVisible = false;
        }

        protected override async Task OnInitializedAsync()
        {
            Users = await UserService.GetUsersAsync();

            await base.OnInitializedAsync();
        }
    }
}
