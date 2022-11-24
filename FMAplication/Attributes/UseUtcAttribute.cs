using FMAplication.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FMAplication.Attributes
{
    public class UseUtcAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //try to modify text
            
            //if (!DateTime.TryParse(value.ToString(), out datetime)) throw new AppException($"{validationContext.MemberName} is not valid date");
            try
            {
                DateTime datetime = (DateTime)value;

                validationContext
                .ObjectType
                .GetProperty(validationContext.MemberName)
                .SetValue(validationContext.ObjectInstance, datetime.ToUniversalTime(), null);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            //return null to make sure this attribute never say iam invalid
            return ValidationResult.Success;
        }
    }
}
