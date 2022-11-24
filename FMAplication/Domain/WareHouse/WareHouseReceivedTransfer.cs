﻿using FMAplication.Core;
using FMAplication.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Domain.WareHouse
{
    public class WareHouseReceivedTransfer:AuditableEntity<int>
    {
        public string TransactionNumber { get; set; }
        public string Remarks { get; set; }
        public bool IsConfirmed { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public DateTime TransactionDate { get; set; }
        public int FromWarehouseId { get; set; }
        public int ToWarehouseId { get; set; }
        public WareHouse ToWarehouse { get; set; }
        public int SourceTransferId { get; set; }

    }
}
