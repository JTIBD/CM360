using System;

namespace FMAplication.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class IgnoreEntityAttribute : Attribute
    {
    }
}
