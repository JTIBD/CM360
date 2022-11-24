using System;

namespace FMAplication.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class IgnoreMigrationAttribute : Attribute
    {
    }
}