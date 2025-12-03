namespace EA.Iws.DataAccess.Mappings.Common
{
    using EA.Iws.Domain;
    using System.Data.Entity.ModelConfiguration;

    internal class MessageBannerMapping : EntityTypeConfiguration<MessageBanner>
    {
        public MessageBannerMapping()
        {
            ToTable("MessageBanner", "Lookup");
        }
    }
}
