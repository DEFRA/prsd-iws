namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment.FinancialGuarantee;
    using Prsd.Core.Helpers;

    public class ImportFinancialGuaranteeMapping : EntityTypeConfiguration<ImportFinancialGuarantee>
    {
        public ImportFinancialGuaranteeMapping()
        {
            ToTable("FinancialGuarantee", "ImportNotification");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<ImportFinancialGuarantee, ICollection<ImportFinancialGuaranteeStatusChange>>(
                    "StatusChangeCollection"))
                .WithRequired()
                .Map(m => m.MapKey("FinancialGuaranteeId"));
        }
    }
}
