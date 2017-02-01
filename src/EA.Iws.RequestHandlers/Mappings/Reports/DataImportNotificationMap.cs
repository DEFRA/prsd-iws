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
                Preconsented = GetPreconsented(source),
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
                Officer = source.Officer,
                CompleteToAcknowledgedElapsedWorkingDays = GetCompleteToAcknowledgedElapsedWorkingDays(source, parameter),
                DecisionToConsentElapsedWorkingDays = GetDecisionToConsentElapsedWorkingDays(source, parameter),
                ReceivedToAcknowledgedElapsedWorkingDays = GetReceivedToAcknowledgedElapsedWorkingDays(source, parameter),
                ReceivedToConsentElapsedWorkingDays = GetReceivedToConsentElapsedWorkingDays(source, parameter)
            };
        }

        private static string GetPreconsented(DataImportNotification source)
        {
            if (source.Preconsented.HasValue)
            {
                return source.Preconsented.Value ? "Yes" : "No";
            }

            return null;
        }

        private DateTime GetReceivedOrPaymentDate(DataImportNotification source)
        {
            if (!source.PaymentReceived.HasValue)
            {
                throw new InvalidOperationException("PaymentReceived has no value");
            }

            if (!source.NotificationReceived.HasValue)
            {
                throw new InvalidOperationException("NotificationReceived has no value");
            }

            return source.PaymentReceived.Value > source.NotificationReceived.Value
                ? source.PaymentReceived.Value
                : source.NotificationReceived.Value;
        }

        private int? GetAssessmentStartedElapsedWorkingDays(DataImportNotification source, UKCompetentAuthority parameter)
        {
            if (!source.PaymentReceived.HasValue || !source.AssessmentStarted.HasValue || !source.NotificationReceived.HasValue)
            {
                return null;
            }

            return workingDayCalculator.GetWorkingDays(
                GetReceivedOrPaymentDate(source), 
                source.AssessmentStarted.Value,
                false, parameter);
        }

        private DateTime? GetDecisionRequiredByDate(DataImportNotification source, UKCompetentAuthority parameter)
        {
            if (!source.Acknowledged.HasValue || !source.Preconsented.HasValue)
            {
                return null;
            }

            return decisionRequiredByCalculator.Get(source.Preconsented.Value, source.Acknowledged.Value, parameter);
        }

        private int? GetReceivedToAcknowledgedElapsedWorkingDays(DataImportNotification source,
            UKCompetentAuthority parameter)
        {
            if (!source.PaymentReceived.HasValue || !source.Acknowledged.HasValue || !source.NotificationReceived.HasValue)
            {
                return null;
            }

            return workingDayCalculator.GetWorkingDays(
                GetReceivedOrPaymentDate(source),
                source.Acknowledged.Value,
                false, parameter);
        }

        private int? GetCompleteToAcknowledgedElapsedWorkingDays(DataImportNotification source,
            UKCompetentAuthority parameter)
        {
            if (!source.ApplicationCompleted.HasValue || !source.Acknowledged.HasValue)
            {
                return null;
            }

            return workingDayCalculator.GetWorkingDays(
                source.ApplicationCompleted.Value,
                source.Acknowledged.Value,
                false, parameter);
        }

        private int? GetReceivedToConsentElapsedWorkingDays(DataImportNotification source,
            UKCompetentAuthority parameter)
        {
            if (!source.PaymentReceived.HasValue || !source.Consented.HasValue || !source.NotificationReceived.HasValue)
            {
                return null;
            }

            return workingDayCalculator.GetWorkingDays(
                GetReceivedOrPaymentDate(source),
                source.Consented.Value,
                false, parameter);
        }

        private int? GetDecisionToConsentElapsedWorkingDays(DataImportNotification source,
            UKCompetentAuthority parameter)
        {
            if (!source.DecisionDate.HasValue || !source.Consented.HasValue)
            {
                return null;
            }

            return workingDayCalculator.GetWorkingDays(
                source.DecisionDate.Value,
                source.Consented.Value,
                false, parameter);
        }
    }
}