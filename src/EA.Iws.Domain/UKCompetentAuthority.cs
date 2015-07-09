namespace EA.Iws.Domain
{
    using System;
    using Prsd.Core.Domain;

    public class UKCompetentAuthority : Enumeration
    {
        public static readonly UKCompetentAuthority England = new UKCompetentAuthority(1, "Environment Agency", "EA");

        public static readonly UKCompetentAuthority Scotland = new UKCompetentAuthority(2,
            "Scottish Environment Protection Agency", "SEPA");

        public static readonly UKCompetentAuthority NorthernIreland = new UKCompetentAuthority(3,
            "Northern Ireland Environment Agency", "NIEA");

        public static readonly UKCompetentAuthority Wales = new UKCompetentAuthority(4, "Natural Resources Wales", "NRW");
        private readonly string shortName;

        protected UKCompetentAuthority()
        {
        }

        private UKCompetentAuthority(int value, string displayName, string shortName)
            : base(value, displayName)
        {
            this.shortName = shortName;
        }

        public string ShortName
        {
            get { return shortName; }
        }

        public Core.Notification.CompetentAuthority AsCompetentAuthority()
        {
            if (Value == England.Value)
            {
                return Core.Notification.CompetentAuthority.England;
            }
            if (Value == Scotland.Value)
            {
                return Core.Notification.CompetentAuthority.Scotland;
            }
            if (Value == NorthernIreland.Value)
            {
                return Core.Notification.CompetentAuthority.NorthernIreland;
            }
            if (Value == Wales.Value)
            {
                return Core.Notification.CompetentAuthority.Wales;
            }

            throw new InvalidOperationException(string.Format("Unknown competent authority {0}", this.DisplayName));
        }
    }
}