using System.Collections.Generic;
using FMAplication.Core;
using System.ComponentModel.DataAnnotations;
using FMAplication.Domain.Users;

namespace FMAplication.Domain.Sales
{
    public class Node : AuditableEntity<int>
    {
        public int NodeId { get; set; }

        //[UniqueKey]
        [StringLength(128, MinimumLength = 1)]
        public string Code { get; set; }
        [Required]
        [StringLength(256, MinimumLength = 1)]
        public string Name { get; set; }

        public int? ParentId { get; set; }
        //[ForeignKey("ParentId")]
        //public Node Parent { get; set; }
        //// [InverseProperty("Parent")]
        //public List<Node> Children { get; set; }
        //public List<SalesPointNodeMapping> SalesPointNodes { get; set; }
       // public List<UserTerritoryMapping> Users { get; set; }

    }
}
