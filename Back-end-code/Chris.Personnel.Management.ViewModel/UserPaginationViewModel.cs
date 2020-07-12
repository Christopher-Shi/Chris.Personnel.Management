using System.Collections.Generic;

namespace Chris.Personnel.Management.ViewModel
{
    public class UserPaginationViewModel : PaginationViewModel<UserPageViewModel>
    {
        public UserPaginationViewModel(IList<UserPageViewModel> list, int currentPage, int pageSize, int total)
            : base(list, currentPage, pageSize, total)
        {
        }
    }
}