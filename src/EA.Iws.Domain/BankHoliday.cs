namespace EA.Iws.Domain
{
    using System;
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;
    
    public class BankHoliday
    {
        public Guid Id { get; private set; }

        public DateTime Date { get; private set; }

        public CompetentAuthorityEnum CompetentAuthority { get; private set; }

        protected BankHoliday()
        {
        }
    }
}
