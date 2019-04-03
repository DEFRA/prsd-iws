namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.Movement.BulkUpload;
    using Prsd.Core.Helpers;

    internal class DraftMovementMapping : EntityTypeConfiguration<DraftMovement>
    {
        public DraftMovementMapping()
        {
            ToTable("Movement", "Draft");

            HasMany(
                    ExpressionHelper
                            .GetPrivatePropertyExpression<DraftMovement, ICollection<DraftPackagingInfo>>(
                                "PackagingInfosCollection"))
                    .WithOptional()
                    .Map(m => m.MapKey("DraftMovementId"));
        }
    }
}
