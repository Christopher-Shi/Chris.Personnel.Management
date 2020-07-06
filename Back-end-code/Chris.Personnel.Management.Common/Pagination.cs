namespace Chris.Personnel.Management.Common
{
    public class Pagination
    {
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int Current { get; set; }

        public Pagination(int pageSize, int total, int current)
        {
            PageSize = pageSize;
            Total = total;
            Current = current;
        }
    }
}
