namespace EA.Iws.RequestHandlers.Mappings.Reports
{
    using System;
    using Core.Admin.Reports;
    using Core.Notification;
    using Domain;
    using Domain.NotificationAssessment;
    using Domain.Reports;
    using Prsd.Core.Mapper;

    internal class DataImportNotificationMap : IMapWithParameter<DataImportNotification, UKCompetentAuthority, DataImportNotificationData>
    {
        private readonly IWorkingDayCalculator workingDayCalculator;
        private readonly IDecisionRequiredByCalculator decisionRequiredByCalculator;

        public DataImportNotificationMap(IWorkingDayCalculator workingDayCalculator, IDecisionRequiredByCalculator decisionRequiredByCalculator)
        {
            this.workingDayCalculator = workingDayCalculator;
            this.decisionRequiredByCalculator = decisionRequiredByCalculator;
        }

        public DataImportNotificationData Map(DataImportNotification source, UKCompetentAuthority parameter)
        {
            return new DataImportNotificationData
            {
                NotificationType = source.NotificationType,
                Status = source.Status,
                NotificationNumber = source.NotificationNumber,
                Acknowledged = source.Acknowledged,
                ApplicationCompleted = source.ApplicationCompleted,
                AssessmentStarted = source.AssessmentStarted,
                DecisionDate = source.DecisionDate,
                NotificationReceived = source.NotificationReceived,
                PaymentReceived = source.PaymentReceived,
                AssessmentStartedElapsedWorkingDays = GetAssessmentStartedElapsedWorkingDays(source, parameter),
                DecisionRequiredDate = GetDecisionRequiredByDate(source, parameter),
                ConsentDate = source.Consented,
                Officer = source.Officer
            };
        }

        private int? GetAssessmentStartedElapsedWorkingDays(DataImportNotification source, UKCompetentAuthority parameter)
        {
            if (!source.PaymentReceived.HasValue || !source.AssessmentStarted.HasValue || !source.NotificationReceived.HasValue)
            {
                return null;
            }

            return workingDayCalculator.GetWorkingDays(
                source.PaymentReceived.Value > source.NotificationReceived.Value ? source.PaymentReceived.Value : source.NotificationReceived.Value, 
                source.AssessmentStarted.Value,
                false, parameter);
        }

        private DateTime? GetDecisionRequiredByDate(DataImportNotification source, UKCompetentAuthority parameter)
        {
            if (!source.Acknowledged.HasValue)
            {
                return null;
            }

            return decisionRequiredByCalculator.Get(source.Preconsented, source.Acknowledged.Value, parameter);
        }
    }
}