using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using FMAplication.Attributes;
using FMAplication.Enumerations;

namespace FMAplication.Core.Params
{
    public class PomsAssignParams
    {
        private const int MaxPageSize = 1000;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int SalesPointId { get; set; }
        [UseUtc]
        public DateTime FromDate { get; set; }
        [UseUtc]
        public DateTime ToDate { get; set; }

        public string Sort { get; set; }
        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
