using JobPortal.Library.Enumerators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;



namespace JobPortal.Library.Validators
{
    public class CompareNumericsAttribute : ValidationAttribute, IClientValidatable
    {
        //private const string lessThanErrorMessage = "{0} must be less than {1}.";
        //private const string lessThanOrEqualToErrorMessage = "{0} must be less than or equal to {1}.";

        public string OtherProperty { get; private set; }
        
        public CompareTypes CompareType { get; private set; }

        public CompareNumericsAttribute(string otherProperty, CompareTypes compareType)
        {
            if (otherProperty == null) { throw new ArgumentNullException("otherProperty"); }
            this.OtherProperty = otherProperty;
            this.CompareType = compareType;
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, this.OtherProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            PropertyInfo otherPropertyInfo = validationContext.ObjectType.GetProperty(OtherProperty);

            if (otherPropertyInfo == null)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "Could not find a property named {0}.", OtherProperty));
            }

            object otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            decimal decv=0;
            decimal deco=0;
            long lngv=0;
            long lngo=0;
            int intv=0;
            int into=0;
            double dblv=0;
            double dblo=0;
            float fltv=0;
            float flto=0;

            if (value != null && !IsNumber<decimal>(value.ToString(), out decv) && value is decimal)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", validationContext.DisplayName));
            }
            else if (value != null && !IsNumber<long>(value.ToString(), out lngv) && value is long)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", validationContext.DisplayName));
            }

            else if (value != null && !IsNumber<int>(value.ToString(), out intv) && value is int)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", validationContext.DisplayName));
            }
            else if (value != null && !IsNumber<double>(value.ToString(), out dblv) && value is double)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", validationContext.DisplayName));
            }

            else if (value != null && !IsNumber<float>(value.ToString(), out fltv) && value is float)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", validationContext.DisplayName));
            }
           
            /* For other value */
            if (otherValue != null && !IsNumber<decimal>(otherValue.ToString(), out deco) && value is decimal)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", OtherProperty));
            }

            else if (otherValue != null && !IsNumber<long>(otherValue.ToString(), out lngo) && value is long)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", OtherProperty));
            }

            else if (otherValue != null && !IsNumber<int>(otherValue.ToString(), out into) && value is int)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", OtherProperty));
            }
            else if (otherValue != null && !IsNumber<double>(otherValue.ToString(), out dblo) && value is double)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", OtherProperty));
            }

            if (otherValue != null && !IsNumber<float>(otherValue.ToString(), out flto) && value is float)
            {
                return new ValidationResult(String.Format(CultureInfo.CurrentCulture, "{0} is not a numeric value.", OtherProperty));
            }        

            switch (CompareType)
            {
                case CompareTypes.Equals:
                    if (decv != deco || lngv != lngo || intv != into || dblv != dblo || fltv != flto)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                    break;
                case CompareTypes.LessThen:
                    if (decv >= deco || lngv >= lngo || intv >= into || dblv >= dblo || fltv >= flto)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                    break;
                case CompareTypes.LessThenEquals:
                    if (decv > deco || lngv > lngo || intv > into || dblv > dblo || fltv > flto)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                    break;
                case CompareTypes.GreaterThan:
                    if (decv <= deco || lngv <= lngo || intv <= into || dblv <= dblo || fltv <= flto)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                    break;
                case CompareTypes.GreaterThanEquals:
                    if (decv < deco || lngv < lngo || intv < into || dblv < dblo || fltv < flto)
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                    break;
            }
            return null;
        }

        private static bool IsNumber<T>(string input, out T output)
        {
            bool flag = false;
            output = default(T);

            if (input != null)
            {
                try
                {
                    output = (T)Convert.ChangeType(input, output.GetType());
                    flag = true;
                }
                catch (Exception)
                {
                    flag = false;
                }
            }
            return flag;
        }
        
        public static string FormatPropertyForClientValidation(string property)
        {
            if (property == null)
            {
                throw new ArgumentException("Value cannot be null or empty.", "property");
            }
            return "*." + property;
        }     

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata,ControllerContext context)
        {
            yield return new ModelClientValidationCompareNumericsRule(ErrorMessageString, FormatPropertyForClientValidation(this.OtherProperty), this.CompareType);
        }

        public static bool ValidateEmaild(string EmailId)
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(EmailId))
            {

                isValid = false;
            }
            else if (!Regex.IsMatch(EmailId, @"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$"))
            {

                isValid = false;
            }

            return isValid;
        }

        public class ModelClientValidationCompareNumericsRule : ModelClientValidationRule
        {
            public ModelClientValidationCompareNumericsRule(string errorMessage, object other, CompareTypes compareType)
            {
                ErrorMessage = errorMessage;
                ValidationType = "comparenumerics";
                ValidationParameters["other"] = other;
                ValidationParameters["type"] = compareType;
            }
        }

        public static string GenerateRandomString(int length)
        {
            //It will generate string with combination of small,capital letters and numbers
            char[] charArr = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();
            string randomString = string.Empty;
            Random objRandom = new Random();
            for (int i = 0; i < length; i++)
            {
                //Don't Allow Repetation of Characters
                int x = objRandom.Next(1, charArr.Length);
                if (!randomString.Contains(charArr.GetValue(x).ToString()))
                    randomString += charArr.GetValue(x);
                else
                    i--;
            }
            return randomString;
        }

    

    }
}
