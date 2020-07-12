using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chris.Personnel.Management.Common;
using Chris.Personnel.Management.QueryService.Specifications;
using Chris.Personnel.Management.Repository;
using Chris.Personnel.Management.ViewModel;
using Chris.Personnel.Management.ViewModel.DropDownListItems;
using Chris.Personnel.Management.ViewModel.Filters;
using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.QueryService.Implements
{
    public class RoleQueryService : IRoleQueryService
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleQueryService(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<RoleViewModel> Get(Guid id)
        {
            var role = await _roleRepository.Get(id);
            return _mapper.Map<RoleViewModel>(role);
        }

        public async Task<RolePaginationViewModel> GetByPage(
            RoleFilters filters, 
            int currentPage, 
            int pageSize, 
            string orderByPropertyName, 
            bool isAsc)
        {
            var specifications = new RoleSpecifications(filters.Name);
            var roles = await _roleRepository.GetAll().Where(specifications.Expression)
                .SortByProperty(orderByPropertyName, isAsc)
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize).ToListAsync();
            var total = _roleRepository.GetAll().Where(specifications.Expression).Count();

            var results = roles.Select(x => _mapper.Map<RolePageViewModel>(x)).ToList();
            return new RolePaginationViewModel(results, currentPage, pageSize, total);
        }

        public async Task<IEnumerable<RoleDropDownListViewModel>> GetRoleForDropDownList()
        {
            return await _roleRepository.GetAll()
                .Select(x => _mapper.Map<RoleDropDownListViewModel>(x)).ToListAsync();
        }
    }
}