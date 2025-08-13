namespace EA.Iws.Domain
{
    using EA.Iws.Core.Notification;
    using EA.Iws.Core.SystemSettings;
    using System;

    public class SystemSetting
    {
        protected SystemSetting()
        {
        }

        public int Id { get; private set; }
        public UKCompetentAuthority CompetentAuthority { get; private set; }
        public SystemSettingType PriceType { get; private set; }
        public DateTime ValidFrom { get; private set; }
        public decimal Price { get; private set; }

        public bool Equals(int id)
        {
            return this.Id == id;
        }
    }
}