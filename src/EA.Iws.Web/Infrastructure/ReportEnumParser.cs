namespace EA.Iws.Web.Infrastructure
{
    using System;
    public static class ReportEnumParser
    {
        public static T? TryParse<T>(string value) where T : struct
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            T result;

            if (Enum.TryParse<T>(value, true, out result) &&
                Enum.IsDefined(typeof(T), result))
            {
                return result;
            }

            return null;
        }
    }
}