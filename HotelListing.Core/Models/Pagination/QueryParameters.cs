namespace HotelListing.Models.NewFolder
{
    public class QueryParameters
    {
        private int _pagesize = 5;
        private const int MaxPageSize = 50;
 
        public int StartIndex => (PageNumber - 1) * PageSize;
        public int PageNumber { get; set; } = 1;
        public int PageSize { 
        
        //get { return _pagesize; }
        get=> _pagesize;

        set => _pagesize = value<MaxPageSize?_pagesize:MaxPageSize;

        }


    }
}
