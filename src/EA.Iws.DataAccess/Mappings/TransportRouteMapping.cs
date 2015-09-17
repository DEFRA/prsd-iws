namespace EA.Iws.DataAccess.Mappings
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.TransportRoute;
    using Prsd.Core.Helpers;

    internal class TransportRouteMapping : EntityTypeConfiguration<TransportRoute>
    {
        public TransportRouteMapping()
        {
            ToTable("TransportRoute", "Notification");

            HasOptional(x => x.StateOfExport)
                .WithRequired()
                .Map(m => m.MapKey("TransportRouteId"));

            HasOptional(x => x.StateOfImport)
                .WithRequired()
                .Map(m => m.MapKey("TransportRouteId"));

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<TransportRoute, ICollection<TransitState>>(
                    "TransitStatesCollection"))
                .WithRequired()
                .Map(m => m.MapKey("TransportRouteId"));

            HasOptional(x => x.ExitCustomsOffice)
                .WithRequired()
                .Map(m => m.MapKey("TransportRouteId"));

            HasOptional(x => x.EntryCustomsOffice)
                .WithRequired()
                .Map(m => m.MapKey("TransportRouteId"));
        }
    }
}