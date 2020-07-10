using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Chris.Personnel.Management.LogicService;
using Chris.Personnel.Management.QueryService;
using Chris.Personnel.Management.ViewModel;
using System.Collections.Generic;
using Chris.Personnel.Management.Common.Enums;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.ViewModel.Filters;

namespace Chris.Personnel.Management.API.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserLogicService _userLogicService;
        private readonly IUserQueryService _userQueryService;

        public UsersController(
            IUserLogicService userLogicService,
            IUserQueryService userQueryService)
        {
            _userLogicService = userLogicService;
            _userQueryService = userQueryService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IEnumerable<UserFormViewModel>> GetUsers()
        {
            return await _userQueryService.GetAll();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<UserFormViewModel> GetUser(Guid id)
        {
            return await _userQueryService.Get(id);
        }

        [Route("pagination")]
        [HttpGet]
        public async Task<UserPaginationViewModel> GetByPage(
            string trueName, Gender? gender, IsEnabled? isEnabled,
            int current, int pageSize, string orderByPropertyName, bool isAsc)
        {
            return await _userQueryService.GetByPage(new UserFilters(trueName, gender, isEnabled),
                current, pageSize, orderByPropertyName, isAsc);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task PutUser(UserEditUICommand command)
        {
            await _userLogicService.Edit(command);
        }

        // POST: api/Users
        [HttpPost]
        public async Task PostUser(UserAddUICommand command)
        {
            await _userLogicService.Add(command);
        }

        [HttpPut("password/modification")]
        public async Task UpdatePassword([FromBody] UserEditPasswordUICommand command)
        {
            await _userLogicService.EditPassword(command);
        }

        [HttpPut("password/resetting")]
        public async Task ResetPassword([FromBody] Guid id)
        {
            await _userLogicService.ResetPassword(id);
        }

        [HttpDelete("{id}/disable")]
        public async Task Disable(Guid id)
        {
            await _userLogicService.StopUsing(new UserDeleteUICommand { Id = id });
        }
    }
}
