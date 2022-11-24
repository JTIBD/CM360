using System;

namespace FMAplication.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class IgnoreUpdateAttribute : Attribute
    {
    }
}