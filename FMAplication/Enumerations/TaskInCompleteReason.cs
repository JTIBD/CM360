using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Enumerations
{
    public enum TaskStatus
    {
        Pending, 
        Complete, 
        InComplete
    }
    
    public enum TaskInCompleteReason
    {
        BusinessOwnerNotInterested,
        NoEnoughPlace,
        BadWeather, 
        OtherCompanyVendor, 
        HasProblemWithLaws, 
        NoEnoughItems
    }


}
