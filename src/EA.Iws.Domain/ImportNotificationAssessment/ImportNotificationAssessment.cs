namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Consent;
    using Core.Admin;
    using Core.ImportNotificationAssessment;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using Stateless;

    public class ImportNotificationAssessment : Entity
    {
        private enum Trigger
        {
            Receive = 1,
            Submit = 2,
            FullyPaid = 3,
            BeginAssessment = 4,
            CompleteNotification = 5,
            Acknowledge = 6,
            Consent = 7,
            WithdrawConsent = 8,
            Object = 9
        }

        private static readonly BidirectionalDictionary<DecisionType, Trigger> DecisionTriggers
            = new BidirectionalDictionary<DecisionType, Trigger>(new Dictionary<DecisionType, Trigger>
            {
                { DecisionType.Consent, Trigger.Consent },
                { DecisionType.Object, Trigger.Object },
                { DecisionType.ConsentWithdrawn, Trigger.WithdrawConsent }
            });

        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTimeOffset> receivedTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTimeOffset> fullyPaidTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTimeOffset, string> beginAssessmentTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTimeOffset> completeNotificationTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTimeOffset> consentedTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTimeOffset> acknowledgeTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTimeOffset, string> withdrawConsentTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTimeOffset, string> objectTrigger;

        private readonly StateMachine<ImportNotificationStatus, Trigger> stateMachine;

        public Guid NotificationApplicationId { get; private set; }

        public ImportNotificationStatus Status { get; private set; }

        public virtual ImportNotificationDates Dates { get; private set; }

        protected virtual ICollection<ImportNotificationStatusChange> StatusChangeCollection { get; private set; }

        public IEnumerable<ImportNotificationStatusChange> StatusChanges
        {
            get { return StatusChangeCollection.ToSafeIEnumerable(); }
        }

        protected ImportNotificationAssessment()
        {
            stateMachine = CreateStateMachine();
        }

        public ImportNotificationAssessment(Guid notificationId)
        {
            NotificationApplicationId = notificationId;
            Dates = new ImportNotificationDates();
            Status = ImportNotificationStatus.New;
            stateMachine = CreateStateMachine();
        }

        private StateMachine<ImportNotificationStatus, Trigger> CreateStateMachine()
        {
            var stateMachine = new StateMachine<ImportNotificationStatus, Trigger>(() => Status, s => Status = s);

            stateMachine.OnTransitioned(OnTransition);

            receivedTrigger = stateMachine.SetTriggerParameters<DateTimeOffset>(Trigger.Receive);
            fullyPaidTrigger = stateMachine.SetTriggerParameters<DateTimeOffset>(Trigger.FullyPaid);
            beginAssessmentTrigger = stateMachine.SetTriggerParameters<DateTimeOffset, string>(Trigger.BeginAssessment);
            completeNotificationTrigger = stateMachine.SetTriggerParameters<DateTimeOffset>(Trigger.CompleteNotification);
            acknowledgeTrigger = stateMachine.SetTriggerParameters<DateTimeOffset>(Trigger.Acknowledge);
            consentedTrigger = stateMachine.SetTriggerParameters<DateTimeOffset>(Trigger.Consent);
            withdrawConsentTrigger = stateMachine.SetTriggerParameters<DateTimeOffset, string>(Trigger.WithdrawConsent);
            objectTrigger = stateMachine.SetTriggerParameters<DateTimeOffset, string>(Trigger.Object);

            stateMachine.Configure(ImportNotificationStatus.New)
                .Permit(Trigger.Receive, ImportNotificationStatus.NotificationReceived);

            stateMachine.Configure(ImportNotificationStatus.AwaitingAssessment)
                .SubstateOf(ImportNotificationStatus.Submitted)
                .OnEntryFrom(fullyPaidTrigger, OnFullyPaid)
                .Permit(Trigger.BeginAssessment, ImportNotificationStatus.InAssessment);

            stateMachine.Configure(ImportNotificationStatus.AwaitingPayment)
                .SubstateOf(ImportNotificationStatus.Submitted)
                .Permit(Trigger.FullyPaid, ImportNotificationStatus.AwaitingAssessment);

            stateMachine.Configure(ImportNotificationStatus.NotificationReceived)
                .OnEntryFrom(receivedTrigger, OnReceived)
                .Permit(Trigger.Submit, ImportNotificationStatus.AwaitingPayment);

            stateMachine.Configure(ImportNotificationStatus.InAssessment)
                .OnEntryFrom(beginAssessmentTrigger, OnAssessmentStarted)
                .Permit(Trigger.CompleteNotification, ImportNotificationStatus.ReadyToAcknowledge)
                .Permit(Trigger.Object, ImportNotificationStatus.Objected);

            stateMachine.Configure(ImportNotificationStatus.ReadyToAcknowledge)
                .OnEntryFrom(completeNotificationTrigger, OnNotificationCompleted)
                .Permit(Trigger.Acknowledge, ImportNotificationStatus.DecisionRequiredBy);

            stateMachine.Configure(ImportNotificationStatus.DecisionRequiredBy)
                .OnEntryFrom(acknowledgeTrigger, OnAcknowledged)
                .Permit(Trigger.Consent, ImportNotificationStatus.Consented)
                .Permit(Trigger.Object, ImportNotificationStatus.Objected);

            stateMachine.Configure(ImportNotificationStatus.Consented)
                .OnEntryFrom(consentedTrigger, OnConsented)
                .Permit(Trigger.WithdrawConsent, ImportNotificationStatus.ConsentWithdrawn);

            stateMachine.Configure(ImportNotificationStatus.ConsentWithdrawn)
                .OnEntryFrom(withdrawConsentTrigger, OnConsentWithdrawn);

            stateMachine.Configure(ImportNotificationStatus.Objected)
                .OnEntryFrom(objectTrigger, OnObjected);

            return stateMachine;
        }

        private void OnConsented(DateTimeOffset consentDate)
        {
            Dates.ConsentedDate = consentDate;
        }

        private void OnConsentWithdrawn(DateTimeOffset withdrawnDate, string reasons)
        {
            Dates.ConsentWithdrawnDate = withdrawnDate;
            Dates.ConsentWithdrawnReasons = reasons;
        }

        private void OnAcknowledged(DateTimeOffset acknowledgedDate)
        {
            Dates.AcknowledgedDate = acknowledgedDate;
        }

        private void OnNotificationCompleted(DateTimeOffset completedDate)
        {
            Dates.NotificationCompletedDate = completedDate;
        }

        private void OnAssessmentStarted(DateTimeOffset date, string officer)
        {
            Dates.AssessmentStartedDate = date;
            Dates.NameOfOfficer = officer;
        }

        private void OnFullyPaid(DateTimeOffset paymentDate)
        {
            Dates.PaymentReceivedDate = paymentDate;
        }

        private void OnTransition(StateMachine<ImportNotificationStatus, Trigger>.Transition transition)
        {
            RaiseEvent(new ImportNotificationStatusChangeEvent(this, transition.Source, transition.Destination));
        }

        private void OnObjected(DateTimeOffset objectionDate, string reason)
        {
            Dates.ObjectedDate = objectionDate;
            Dates.ObjectedReason = reason;
        }

        public void AddStatusChangeRecord(ImportNotificationStatusChange statusChange)
        {
            Guard.ArgumentNotNull(() => statusChange, statusChange);

            StatusChangeCollection.Add(statusChange);
        }

        public void Receive(DateTimeOffset receivedDate)
        {
            stateMachine.Fire(receivedTrigger, receivedDate);
        }

        private void OnReceived(DateTimeOffset receivedDate)
        {
            Dates.NotificationReceivedDate = receivedDate;
        }

        public void Submit()
        {
            stateMachine.Fire(Trigger.Submit);
        }

        public void PaymentComplete(DateTimeOffset date)
        {
            stateMachine.Fire(fullyPaidTrigger, date);
        }

        public void BeginAssessment(DateTimeOffset date, string officer)
        {
            stateMachine.Fire(beginAssessmentTrigger, date, officer);
        }

        public void CompleteNotification(DateTimeOffset date)
        {
            stateMachine.Fire(completeNotificationTrigger, date);
        }

        public void Acknowledge(DateTimeOffset date)
        {
            stateMachine.Fire(acknowledgeTrigger, date);
        }

        public void WithdrawConsent(DateTime withdrawalDate, string reasonsForWithdrawal)
        {
            stateMachine.Fire(withdrawConsentTrigger, withdrawalDate, reasonsForWithdrawal);
        }

        public IEnumerable<DecisionType> GetAvailableDecisions()
        {
            var triggers = stateMachine.PermittedTriggers
                .Where(t => DecisionTriggers.ContainsKey(t))
                .Select(t => DecisionTriggers[t]);

            return triggers;
        }

        internal ImportConsent Consent(DateTimeOffsetRange dateRange,
            string conditions,
            Guid userId,
            DateTimeOffset consentedDate)
        {
            stateMachine.Fire(consentedTrigger, consentedDate);

            return new ImportConsent(NotificationApplicationId, dateRange, conditions, userId);
        }

        public void Object(DateTimeOffset objectedDate, string reason)
        {
            stateMachine.Fire(objectTrigger, objectedDate, reason);
        }
    }
}
