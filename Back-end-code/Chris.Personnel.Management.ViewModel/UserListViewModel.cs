using System.Collections.Generic;

namespace Chris.Personnel.Management.ViewModel
{
    public class UserListViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TrueName { get; set; }
        public string Gender { get; set; }
        public string CardId { get; set; }
        public string Phone { get; set; }
        public string IsEnabled { get; set; }
        public string LastModifiedTime { get; set; }
    }

    public class UserPaginationViewModel : PaginationViewModel<UserListViewModel>
    {
        public UserPaginationViewModel(IList<UserListViewModel> list, int currentPage, int pageSize, int total)
            : base(list, currentPage, pageSize, total)
        {
        }
    }
}
