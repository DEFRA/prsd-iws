namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotification;
    using Prsd.Core.Helpers;

    internal class WasteOperationMapping : EntityTypeConfiguration<WasteOperation>
    {
        public WasteOperationMapping()
        {
            ToTable("WasteOperation", "ImportNotification");

            Property(x => x.TechnologyEmployed).HasMaxLength(70);

            HasMany(ExpressionHelper.GetPrivatePropertyExpression<WasteOperation, ICollection<WasteOperationCode>>("OperationCodesCollection"))
                .WithRequired()
                .Map(m => m.MapKey("WasteOperationId"));
        }
    }
}