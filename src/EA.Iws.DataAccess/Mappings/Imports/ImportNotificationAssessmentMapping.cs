namespace EA.Iws.DataAccess.Mappings.Imports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.ImportNotificationAssessment;
    using Prsd.Core.Helpers;

    internal class ImportNotificationAssessmentMapping : EntityTypeConfiguration<ImportNotificationAssessment>
    {
        public ImportNotificationAssessmentMapping()
        {
            ToTable("NotificationAssessment", "ImportNotification");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<ImportNotificationAssessment, ICollection<ImportNotificationStatusChange>>(
                    "StatusChangeCollection"))
                .WithRequired()
                .Map(m => m.MapKey("NotificationAssessmentId"));

            HasRequired(x => x.Dates)
                .WithRequiredPrincipal()
                .Map(m => m.MapKey("NotificationAssessmentId"));
        }
    }
}
