using System;
using System.ComponentModel.DataAnnotations.Schema;
using FMAplication.Attributes;
using FMAplication.Enumerations;
using FMAplication.Extensions;

namespace FMAplication.Core
{
    [IgnoreEntity]
    public abstract class AuditableEntity<T> : Entity<T>, IAuditableEntity
    {
        protected AuditableEntity()
        {
            ModifiedBy = 0;
            ModifiedTime = DateTime.UtcNow;
            // IsActive = true;
            CreatedBy = 0;
            CreatedTime = DateTime.UtcNow;
            Status = Status.Active;
        }

        [IgnoreUpdate]
        public int CreatedBy { get; set; }

        [IgnoreUpdate]
        public DateTime CreatedTime { get; set; }
        [NotMapped]
        public string CreatedTimeStr => CreatedTime.ToIsoString();

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedTime { get; set; }
        [NotMapped]
        public string ModifiedTimeStr =>  ModifiedTime?.ToIsoString() ?? "";

        public Status Status { get; set; }
    }
}
