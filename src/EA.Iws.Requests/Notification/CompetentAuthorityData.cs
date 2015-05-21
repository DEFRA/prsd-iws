namespace EA.Iws.Requests.Notification
{
    using System;

    public class CompetentAuthorityData
    {
        public Guid NotificationId { get; set; }

        public CompetentAuthority CompetentAuthority { get; set; }

        public string NotificationNumber { get; set; }

        public string CompetentAuthorityName { get; set; }
    }
}
