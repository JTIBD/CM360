using FMAplication.Attributes;

namespace FMAplication.Core
{
    [IgnoreEntity]
    public interface IEntity<T> : IBaseEntity
    {
        T Id { get; set; }

    }

}
