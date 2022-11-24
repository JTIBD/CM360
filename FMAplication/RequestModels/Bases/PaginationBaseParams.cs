
namespace FMAplication.RequestModels.Bases
{
    public class PaginationBaseParams
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public string Search { get; set; }
    }
}