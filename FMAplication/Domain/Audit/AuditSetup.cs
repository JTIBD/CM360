using FMAplication.Core;
using FMAplication.Domain.Products;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.Bases;

namespace FMAplication.Domain.Audit
{
    public class AuditSetup: BaseSetup
    {
        public string Code { get; set; }
    }
}
