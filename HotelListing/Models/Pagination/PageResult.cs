namespace HotelListing.Models.Pagination
{
    public class PageResult<T>
    {
        public int CurrentPage { get; set; }
       public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int RecordNumber { get; set; }
        public List <T> Items { get; set; }
    }
}
