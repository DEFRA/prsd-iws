namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings.ReceiptRecoveryMappings
{
    using System.Text.RegularExpressions;
    using Core.Movement.BulkReceiptRecovery;
    using Prsd.Core.Mapper;

    public class NotificationNumberDataRowMap : IMap<ReceiptRecoveryDataRow, string>
    {
        private static readonly Regex NotificationNumberRegex = new Regex(@"(GB)(\d{4})(\d{6})", RegexOptions.Compiled);

        public string Map(ReceiptRecoveryDataRow source)
        {
            string result = null;

            try
            {
                result = source.DataRow.ItemArray[(int)ReceiptRecoveryColumnIndex.NotificationNumber].ToString();
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
            else
            {
                number = string.Empty;
            }

            return number;
        }
    }
}
