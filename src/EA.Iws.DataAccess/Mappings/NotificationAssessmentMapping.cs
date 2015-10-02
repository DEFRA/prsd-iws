namespace EA.Iws.DataAccess.Mappings
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.NotificationAssessment;
    using Prsd.Core.Helpers;

    internal class NotificationAssessmentMapping : EntityTypeConfiguration<NotificationAssessment>
    {
        public NotificationAssessmentMapping()
        {
            ToTable("NotificationAssessment", "Notification");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<NotificationAssessment, ICollection<NotificationStatusChange>>(
                    "StatusChangeCollection"))
                .WithRequired()
                .Map(m => m.MapKey("NotificationAssessmentId"));

            HasRequired(x => x.Dates)
                .WithRequiredPrincipal()
                .Map(m => m.MapKey("NotificationAssessmentId"));
        }
    }
}