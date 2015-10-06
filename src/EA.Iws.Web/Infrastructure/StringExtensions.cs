namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static bool IsValidMoneyDecimal(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            Regex rgx = new Regex(@"^(?=[\d.])\d{0,3}(?:\d*|(?:,\d{3})*)(?:\.\d{1,2})?$");

            return rgx.IsMatch(value);
        }

        public static decimal ToMoneyDecimal(this string value)
        {
            if (!value.IsValidMoneyDecimal())
            {
                throw new ArgumentException("Supplied value does not represent a valid decimal", "value");
            }

            value = value.Replace(",", string.Empty);

            decimal result;
            if (!decimal.TryParse(value, out result))
            {
                throw new ArgumentException("Supplied value does not represent a valid decimal", "value");
            }

            return result;
        }
    }
}