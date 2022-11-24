using FMAplication.Core;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;
using FMAplication.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Domain.Bases;

namespace FMAplication.Domain.AVCommunications
{
    public class AvSetup: BaseSetup
    {
        public string Code { get; set; }
        public int AvId { get; set; }
        public AvCommunication Av { get; set; }
    }
}
