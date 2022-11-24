using FMAplication.Core;

namespace FMAplication.Domain.Sales
{

    public class SalesPointNodeMapping : AuditableEntity<int>
    {

        public int NodeId { get; set; }

        //[ForeignKey("NodeId")]
        //public Node Node { get; set; }

        public int SalesPointId { get; set; }

        //[ForeignKey("SalesPointId")]
        //public SalesPoint SalesPoint { get; set; }


    }
}
