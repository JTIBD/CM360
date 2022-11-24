using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FMAplication.RequestModels.Reports
{
    public class GetExecutionReport
    {
        [Required]
        public List<int> SalesPointIds { get; set; }

        [Required] public DateTime FromDateTime { get; set; }
        [Required] public DateTime ToDateTime { get; set; }

    }
}