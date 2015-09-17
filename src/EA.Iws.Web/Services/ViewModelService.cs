namespace EA.Iws.Web.Services
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    public static class ViewModelService
    {
        public static bool IsNumberOfShipmentsValid(string numberOfShipmentsString)
        {
            int numberOfShipments;
            if (numberOfShipmentsString.Contains(","))
            {
                Regex rgx = new Regex(@"^(?=[\d.])\d{0,3}(?:\d*|(?:,\d{3})*)?$");
                if (rgx.IsMatch(numberOfShipmentsString))
                {
                    numberOfShipmentsString = numberOfShipmentsString.Replace(",", string.Empty);
                }
                else
                {
                    return false;
                }
            }

            if (!Int32.TryParse(numberOfShipmentsString, out numberOfShipments))
            {
                return false;
            }

            if (numberOfShipments < 1 || numberOfShipments > 99999)
            {
                return false;
            }
            return true;
        }

        public static bool IsStringValidDecimalToNDecimalPlaces(string s, int n)
        {
            decimal quantity;
            NumberStyles style = NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-GB");

            if (!decimal.TryParse(s, style, culture, out quantity))
            {
                return false;
            }

            return IsDecimalValidToNDecimalPlaces(quantity, n);
        }

        public static bool IsDecimalValidToNDecimalPlaces(decimal? d, int n)
        {
            if (!d.HasValue)
            {
                return false;
            }

            return decimal.Round(d.Value, n) == d.Value;
        }
    }
}