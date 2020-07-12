using System.Collections.Generic;

namespace Chris.Personnel.Management.ViewModel
{
    public class RolePaginationViewModel : PaginationViewModel<RolePageViewModel>
    {
        public RolePaginationViewModel(IList<RolePageViewModel> list, int currentPage, int pageSize, int total)
            : base(list, currentPage, pageSize, total)
        {
        }
    }
}
