﻿namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationConsent;

    internal class ConsentMapping : EntityTypeConfiguration<Consent>
    {
        public ConsentMapping()
        {
            this.ToTable("Consent", "Notification");

            Property(x => x.Conditions).HasMaxLength(4000);
        }
    }
}
