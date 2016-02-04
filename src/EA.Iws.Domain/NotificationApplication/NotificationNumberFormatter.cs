namespace EA.Iws.Domain.NotificationApplication
{
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public static class NotificationNumberFormatter
    {
        private const string NotificationNumberFormat = "GB 000{0} {1}";

        public static string GetNumber(int number, CompetentAuthorityEnum competentAuthority)
        {
            return string.Format(NotificationNumberFormat, (int)competentAuthority, number.ToString("D6"));
        }
    }
}