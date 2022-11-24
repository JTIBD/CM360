using System;
using System.Collections.Generic;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Domain.Users;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using FMAplication.Models.Bases;
using FMAplication.Models.Products;
using FMAplication.Models.Sales;

namespace FMAplication.Models.PosmAssign
{
    public class PosmAssignModel : IWithSalesPoint
    {
        public int Id { get; set; }
        public int SalesPointId { get; set; }
        public SalesPointModel SalesPoint { get; set; }

        public int PosmProductId { get; set; }
        public POSMProduct PosmProduct { get; set; }


        public int CmUserId { get; set; }
        public CmUserModel CmUser { get; set; }

        public PosmTaskAssignStatus TaskStatus { get; set; }


        public DateTime Date { get; set; }
        public string DateStr => Date.ToIsoString();
        public int Quantity { get; set; }

        public int SumOfQuantity { get; set; }
        public int Lines { get; set; }
    }
}
