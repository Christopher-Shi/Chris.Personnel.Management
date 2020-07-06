using System.Collections.Generic;
using Chris.Personnel.Management.Common;

namespace Chris.Personnel.Management.ViewModel
{
    public class PaginationViewModel<T>
    {
        public PaginationViewModel(IList<T> list, int currentPage, int pageSize, int total)
        {
            List = list;
            Pagination = new Pagination(pageSize, total, currentPage);
        }

        public IList<T> List { get; set; }
        public Pagination Pagination { get; set; }
    }
}
