using System;
using System.ComponentModel.DataAnnotations;
using FMAplication.Attributes;

namespace FMAplication.RequestModels.Bases
{
    public class PaginationParams: PaginationBaseParams
    {
        [Required]
        [UseUtc]
        public DateTime FromDateTime { get; set; }
        [Required]
        [UseUtc]
        public DateTime ToDateTime { get; set; }
    }
}