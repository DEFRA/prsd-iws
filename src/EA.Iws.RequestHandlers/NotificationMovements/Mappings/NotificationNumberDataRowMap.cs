namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System.Data;
    using System.Text.RegularExpressions;
    using Core.Movement.Bulk;
    using Prsd.Core.Mapper;

    public class NotificationNumberDataRowMap : IMap<DataRow, string>
    {
        private static readonly Regex NotificationNumberRegex = new Regex(@"(GB)(\d{4})(\d{6})", RegexOptions.Compiled);

        public string Map(DataRow source)
        {
            string result = null;

            try
            {
                result = source.ItemArray[(int)PrenotificationColumnIndex.NotificationNumber].ToString();
            }
            catch
            {
                //ignored
            }

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
