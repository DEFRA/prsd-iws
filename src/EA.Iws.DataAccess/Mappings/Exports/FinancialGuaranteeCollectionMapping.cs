namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Helpers;

    internal class FinancialGuaranteeCollectionMapping : EntityTypeConfiguration<FinancialGuaranteeCollection>
    {
        public FinancialGuaranteeCollectionMapping()
        {
            ToTable("FinancialGuaranteeCollection", "Notification");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<FinancialGuaranteeCollection, ICollection<FinancialGuarantee>>(
                    "FinancialGuaranteesCollection"))
                .WithRequired()
                .Map(m => m.MapKey("FinancialGuaranteeCollectionId"));
        }
    }
}