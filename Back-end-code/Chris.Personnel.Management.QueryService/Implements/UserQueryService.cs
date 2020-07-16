using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Chris.Personnel.Management.Common.Extensions;
using Chris.Personnel.Management.QueryService.Specifications;
using Chris.Personnel.Management.Repository;
using Chris.Personnel.Management.ViewModel;
using Chris.Personnel.Management.ViewModel.DropDownListItems;
using Chris.Personnel.Management.ViewModel.Filters;
using Microsoft.EntityFrameworkCore;

namespace Chris.Personnel.Management.QueryService.Implements
{
    public class UserQueryService : IUserQueryService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserQueryService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserFormViewModel> Get(Guid id)
        {
            var user = await _userRepository.Get(id);
            var userViewModel = _mapper.Map<UserFormViewModel>(user);

            return userViewModel;
        }

        public async Task<List<UserFormViewModel>> GetAll()
        {
            var users = await _userRepository.GetAll().ToListAsync();
            var userViewModels = users.Select(user => _mapper.Map<UserFormViewModel>(user));

            return userViewModels.ToList();
        }

        public async Task<IEnumerable<UserDropDownListViewModel>> GetForDropDownList()
        {
            var uses = await _userRepository.GetAll()
                .Select(x => _mapper.Map<UserDropDownListViewModel>(x)).ToListAsync();
            return uses;
        }

        public async Task<UserPaginationViewModel> GetByPage(
            UserFilters filters,
            int currentPage, 
            int pageSize, 
            string orderByPropertyName,
            bool isAsc)
        {
            var specifications = new UserSpecifications(filters.TrueName, filters.Gender, filters.IsEnabled);
            var users = await _userRepository.GetAll().Where(specifications.Expression)
                .SortByProperty(orderByPropertyName, isAsc)
                .Skip(pageSize * (currentPage - 1))
                .Take(pageSize).ToListAsync();
            var total = _userRepository.GetAll().Where(specifications.Expression).Count();

            var results = users.Select(x => _mapper.Map<UserPageViewModel>(x)).ToList();
            return new UserPaginationViewModel(results, currentPage, pageSize, total);
        }
    }
}
