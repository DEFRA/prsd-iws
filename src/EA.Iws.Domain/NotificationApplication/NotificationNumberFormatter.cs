namespace EA.Iws.Domain.NotificationApplication
{
    public static class NotificationNumberFormatter
    {
        private const string NotificationNumberFormat = "GB 000{0} {1}";

        public static string GetNumber(int number, UKCompetentAuthority competentAuthority)
        {
            return string.Format(NotificationNumberFormat, competentAuthority.Value, number.ToString("D6"));
        }
    }
}