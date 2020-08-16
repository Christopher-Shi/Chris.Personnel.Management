﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BootstrapBlazor.Components;
using Chris.Personnel.Management.Blazor.Services;
using Chris.Personnel.Management.Common.DropDownListItems;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.UICommand.DTO;
using Chris.Personnel.Management.ViewModel;
using Chris.Personnel.Management.ViewModel.DropDownListItems;
using Microsoft.AspNetCore.Components;

namespace Chris.Personnel.Management.Blazor.Pages
{
    public partial class User
    {
        [Inject] public IUserService UserService { get; set; }

        [Inject] public IDropDownService DropDownService { get; set; }

        public IEnumerable<UserFormViewModel> Users { get; set; } = new List<UserFormViewModel>();

        public IEnumerable<GenderDropDownListItem> GenderEnumItems { get; set; } = new List<GenderDropDownListItem>();
        public IEnumerable<IsEnabledDropDownListItem> IsEnabledEnumItems { get; set; } = new List<IsEnabledDropDownListItem>();

        public IEnumerable<RoleDropDownListViewModel> RoleEnumItems { get; set; } = new List<RoleDropDownListViewModel>();
        public IEnumerable<SelectedItem> RoleSelectedItems { get; set; } = new List<SelectedItem>();

        public UserFormViewModel SearchModel { get; set; } = new UserFormViewModel();

        public Modal AddModal { get; set; }
        public Modal EditModal { get; set; }

        public UserAddUICommand UserAdd { get; set; } = new UserAddUICommand
        {
            User = new UserDTO()
        };

        protected override async Task OnInitializedAsync()
        {
            Users = await UserService.GetUsersAsync();
            GenderEnumItems = await DropDownService.GetGenderEnumItemsAsync();
            IsEnabledEnumItems = await DropDownService.GetIsEnabledEnumItemsAsync();
            RoleEnumItems = await DropDownService.GetRoleEnumItemsAsync();

            await base.OnInitializedAsync();
        }

        public void ButtonClick()
        {
            var trueName = UserAdd.User.TrueName;
            var userName = UserAdd.User.UserName;
            var gender = UserAdd.User.Gender;
            var roleId = UserAdd.User.RoleId;
            var phoneNumber = UserAdd.User.Phone;
            var IDCard = UserAdd.User.CardId;
        }

        protected IEnumerable<SelectedItem> Items => new SelectedItem[]
        {
            new SelectedItem("1", "男") { Active = true },
            new SelectedItem("0", "女")
        };


        //protected Task OnItemChanged()
        //{

        //}
    }
}
