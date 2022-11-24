using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FMAplication.Attributes;

namespace FMAplication.RequestModels.Reports
{
    public class ExportSPWisePosmLedgerPayload
    {
        [Required]
        [UseUtc]
        public DateTime FromDateTime { get; set; }
        [Required]
        [UseUtc]
        public DateTime ToDateTime { get; set; }
        [Required]
        public List<int> SalesPointIds { get; set; }
        [Required]
        public List<int> PosmProductIds { get; set; }

    }
}