using System;
using FMAplication.Attributes;
using FMAplication.Enumerations;

namespace FMAplication.Core
{
    [IgnoreEntity]
    public interface IAuditableEntity
    {
        //  bool IsActive { get; set; }
        int CreatedBy { get; set; }
        DateTime CreatedTime { get; set; }
        int? ModifiedBy { get; set; }
        DateTime? ModifiedTime { get; set; }


    }
}
