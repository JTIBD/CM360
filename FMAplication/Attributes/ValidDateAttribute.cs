using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FMAplication.Exceptions;

namespace FMAplication.Attributes
{
    public class ValidDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            try
            {
                DateTime dateTime;
                var dateTimeString = (string)value;
                if (DateTime.TryParse(dateTimeString, out dateTime))
                {

                }
                else
                {
                    if (value != null)
                    {
                        var propertyName = validationContext.MemberName;
                        throw new DefaultException("Not a valid Datetime format for " + propertyName);
                    }
                }
                   
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
