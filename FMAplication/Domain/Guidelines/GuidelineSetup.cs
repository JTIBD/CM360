using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Core;
using FMAplication.Domain.Bases;
using FMAplication.Domain.Sales;
using FMAplication.Enumerations;

namespace FMAplication.Domain.Guidelines
{
    public class GuidelineSetup : BaseSetup
    {
        public string Code { get; set; }
        public string GuidelineText { get; set; }
    }
}
