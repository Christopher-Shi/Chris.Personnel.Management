using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Chris.Personnel.Management.Common.DropDownListItems;
using Chris.Personnel.Management.QueryService;
using Chris.Personnel.Management.ViewModel.DropDownListItems;
using Microsoft.AspNetCore.Mvc;

namespace Chris.Personnel.Management.API.Controllers
{
    public class DropDownController : BaseController
    {
        private readonly IUserQueryService _userQueryService;
        private readonly IRoleQueryService _roleQueryService;

        public DropDownController(
            IUserQueryService userQueryService,
            IRoleQueryService roleQueryService)
        {
            _userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
            _roleQueryService = roleQueryService ?? throw new ArgumentNullException(nameof(roleQueryService));
        }

        [HttpGet("genders")]
        public IEnumerable<GenderDropDownListItem> GetGenderEnumItems()
        {
            return DropDownListItemsCreator.GetGenderDropDownListItems();
        }

        [HttpGet("enabledStatus")]
        public IEnumerable<IsEnabledDropDownListItem> GetIsEnabledEnumItems()
        {
            return DropDownListItemsCreator.GetEnabledDropDownListItems();
        }

        [HttpGet("users")]
        public async Task<IEnumerable<UserDropDownListViewModel>> GetUserEnumItems()
        {
            return await _userQueryService.GetForDropDownList();
        }

        [HttpGet("roles")]
        public async Task<IEnumerable<RoleDropDownListViewModel>> GetRoleEnumItems()
        {
            return await _roleQueryService.GetRoleForDropDownList();
        }
    }
}
