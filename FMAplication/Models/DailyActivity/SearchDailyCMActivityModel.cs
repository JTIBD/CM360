using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System;
using FMAplication.Enumerations;
using FMAplication.Models.Products;
using FMAplication.Models.DailyPOSM;
using FMAplication.Models.DailyAudit;

namespace fm_application.Models.DailyActivity
{
    public class SearchDailyCMActivityModel
    {      
        public SearchDailyCMActivityModel()
        {
            this.Date = DateTime.Now;
        }
        public int CMId { get; set; }
        public DateTime Date { get; set; }
        public int AssignedFMUserId { get; set; }        
    }   
}