namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Data;
    using System.Text.RegularExpressions;
    using Prsd.Core.Mapper;
    public class NotificationNumberDataRowMap : IMap<DataRow, string>
    {
        private const int ColumnIndex = 0;
        private static readonly Regex NotificationNumberRegex = new Regex(@"(GB)(\d{4})(\d{6})", RegexOptions.Compiled);

        public string Map(DataRow source)
        {
            var result = source.Field<string>(ColumnIndex);
            return FormatNotificationNumber(result);
        }

        private static string FormatNotificationNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return string.Empty;
            }

            number = number.ToUpper().Replace(" ", string.Empty);

            if (NotificationNumberRegex.IsMatch(number))
            {
                number = NotificationNumberRegex.Replace(number, "$1 $2 $3");
            }

            return number;
        }
    }
}
