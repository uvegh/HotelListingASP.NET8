namespace HotelListing.Models.NewFolder
{
    public class QueryParameters
    {
        private int _pagesize = 10;

        public int StartIndex { get; set; }

        public int PageSize { 
        
        //get { return _pagesize; }
        get=> _pagesize;

        set => _pagesize = value;

        }


    }
}
