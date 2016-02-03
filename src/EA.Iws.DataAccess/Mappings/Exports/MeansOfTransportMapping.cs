namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationApplication;
    using Prsd.Core.Helpers;

    internal class MeansOfTransportMapping : EntityTypeConfiguration<MeansOfTransport>
    {
        public MeansOfTransportMapping()
        {
            ToTable("MeansOfTransport", "Notification");

            Property(ExpressionHelper
                .GetPrivatePropertyExpression<MeansOfTransport, string>("MeansOfTransportInternal"))
                .HasColumnName("MeansOfTransport")
                .IsRequired();

            Ignore(x => x.Route);
        }
    }
}