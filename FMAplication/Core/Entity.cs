using FMAplication.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FMAplication.Core
{
    [IgnoreEntity]
    public abstract class Entity<T> : IEntity<T>
    {
        [Key]
        public virtual T Id { get; set; }

    }

}
