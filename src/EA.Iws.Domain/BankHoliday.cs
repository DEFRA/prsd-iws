namespace EA.Iws.Domain
{
    using System;

    public class BankHoliday
    {
        public Guid Id { get; private set; }

        public DateTime Date { get; private set; }

        public UKCompetentAuthority CompetentAuthority { get; private set; }

        protected BankHoliday()
        {
        }
    }
}
