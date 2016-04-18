namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;
    using Prsd.Core.Helpers;

    internal class TransportRouteMapping : EntityTypeConfiguration<TransportRoute>
    {
        public TransportRouteMapping()
        {
            ToTable("TransportRoute", "ImportNotification");

            HasOptional(x => x.StateOfExport)
                .WithRequired()
                .Map(m => m.MapKey("TransportRouteId"));

            HasOptional(x => x.StateOfImport)
                .WithRequired()
                .Map(m => m.MapKey("TransportRouteId"));

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<TransportRoute, ICollection<TransitState>>(
                    "TransitStateCollection"))
                .WithRequired()
                .Map(m => m.MapKey("TransportRouteId"));
        }
    }
}