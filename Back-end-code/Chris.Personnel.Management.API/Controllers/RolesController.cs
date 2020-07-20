using System;
using System.Threading.Tasks;
using Chris.Personnel.Management.LogicService;
using Chris.Personnel.Management.QueryService;
using Chris.Personnel.Management.UICommand;
using Chris.Personnel.Management.ViewModel;
using Chris.Personnel.Management.ViewModel.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Chris.Personnel.Management.API.Controllers
{
    public class RolesController : BaseController
    {
        private readonly IRoleLogicService _roleLogicService;
        private readonly IRoleQueryService _roleQueryService;

        public RolesController(
            IRoleLogicService roleLogicService, 
            IRoleQueryService roleQueryService)
        {
            _roleLogicService = roleLogicService ?? throw new ArgumentNullException(nameof(roleLogicService));
            _roleQueryService = roleQueryService ?? throw new ArgumentNullException(nameof(roleQueryService));
        }

        // GET api/roles/id
        [HttpGet("{id}")]
        public async Task<RoleViewModel> Get(Guid id)
        {
            return await _roleQueryService.Get(id);
        }

        // GET api/roles/pagination
        [HttpGet("pagination")]
        public async Task<RolePaginationViewModel> GetByPage(
            string name, 
            int currentPage,
            int pageSize, 
            string orderByPropertyName, 
            bool isAsc)
        {
            return await _roleQueryService.GetByPage(
                new RoleFilters(name), 
                currentPage, 
                pageSize, 
                orderByPropertyName,
                isAsc);
        }

        // POST api/roles
        [HttpPost]
        public async Task Post([FromBody] RoleAddUICommand command)
        {
            await _roleLogicService.Add(command);
        }

        // PUT api/roles/id
        [HttpPut("{id}")]
        public async Task Put(Guid id, [FromBody] RoleEditUICommand command)
        {
            command.Id = id;
            await _roleLogicService.Edit(command);
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await _roleLogicService.Delete(new RoleDeleteUICommand { Id = id});
        }
    }
}
