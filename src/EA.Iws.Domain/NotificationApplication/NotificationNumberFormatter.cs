namespace EA.Iws.Domain.NotificationApplication
{
    using Core.Notification;

    public static class NotificationNumberFormatter
    {
        private const string NotificationNumberFormat = "GB 000{0} {1}";

        public static string GetNumber(int number, UKCompetentAuthority competentAuthority)
        {
            return string.Format(NotificationNumberFormat, (int)competentAuthority, number.ToString("D6"));
        }
    }
}