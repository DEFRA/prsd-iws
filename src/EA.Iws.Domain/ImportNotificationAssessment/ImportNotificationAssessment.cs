namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Consent;
    using Core.Admin;
    using Core.ImportNotificationAssessment;
    using Core.Shared;
    using Decision;
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
            Object = 9,
            Withdraw = 10,
            FileClosed = 11
        }

        private static readonly BidirectionalDictionary<DecisionType, Trigger> DecisionTriggers
            = new BidirectionalDictionary<DecisionType, Trigger>(new Dictionary<DecisionType, Trigger>
            {
                { DecisionType.Consent, Trigger.Consent },
                { DecisionType.Object, Trigger.Object },
                { DecisionType.ConsentWithdraw, Trigger.WithdrawConsent },
                { DecisionType.Withdraw, Trigger.Withdraw }
            });

        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTime> receivedTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTime> fullyPaidTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTime, string> beginAssessmentTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTime> completeNotificationTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTime> consentedTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTime> acknowledgeTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTime, string> withdrawConsentTrigger;
        private StateMachine<ImportNotificationStatus, Trigger>.TriggerWithParameters<DateTime> fileClosedTrigger;

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

            receivedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Receive);
            fullyPaidTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.FullyPaid);
            beginAssessmentTrigger = stateMachine.SetTriggerParameters<DateTime, string>(Trigger.BeginAssessment);
            completeNotificationTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.CompleteNotification);
            acknowledgeTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Acknowledge);
            consentedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Consent);
            withdrawConsentTrigger = stateMachine.SetTriggerParameters<DateTime, string>(Trigger.WithdrawConsent);
            fileClosedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.FileClosed);

            stateMachine.Configure(ImportNotificationStatus.New)
                .Permit(Trigger.Receive, ImportNotificationStatus.NotificationReceived);

            stateMachine.Configure(ImportNotificationStatus.NotificationReceived)
                .OnEntryFrom(receivedTrigger, OnReceived)
                .Permit(Trigger.Submit, ImportNotificationStatus.AwaitingPayment)
                .Permit(Trigger.Withdraw, ImportNotificationStatus.Withdrawn)
                .Permit(Trigger.Object, ImportNotificationStatus.Objected)
                .Permit(Trigger.FileClosed, ImportNotificationStatus.FileClosed);

            stateMachine.Configure(ImportNotificationStatus.AwaitingPayment)
                .SubstateOf(ImportNotificationStatus.Submitted)
                .Permit(Trigger.FullyPaid, ImportNotificationStatus.AwaitingAssessment)
                .Permit(Trigger.Withdraw, ImportNotificationStatus.Withdrawn)
                .Permit(Trigger.Object, ImportNotificationStatus.Objected)
                .Permit(Trigger.FileClosed, ImportNotificationStatus.FileClosed);

            stateMachine.Configure(ImportNotificationStatus.AwaitingAssessment)
                .SubstateOf(ImportNotificationStatus.Submitted)
                .OnEntryFrom(fullyPaidTrigger, OnFullyPaid)
                .Permit(Trigger.BeginAssessment, ImportNotificationStatus.InAssessment)
                .Permit(Trigger.Withdraw, ImportNotificationStatus.Withdrawn)
                .Permit(Trigger.Object, ImportNotificationStatus.Objected)
                .Permit(Trigger.FileClosed, ImportNotificationStatus.FileClosed);
            
            stateMachine.Configure(ImportNotificationStatus.InAssessment)
                .OnEntryFrom(beginAssessmentTrigger, OnAssessmentStarted)
                .Permit(Trigger.CompleteNotification, ImportNotificationStatus.ReadyToAcknowledge)
                .Permit(Trigger.Object, ImportNotificationStatus.Objected)
                .Permit(Trigger.Withdraw, ImportNotificationStatus.Withdrawn)
                .Permit(Trigger.FileClosed, ImportNotificationStatus.FileClosed);

            stateMachine.Configure(ImportNotificationStatus.ReadyToAcknowledge)
                .OnEntryFrom(completeNotificationTrigger, OnNotificationCompleted)
                .Permit(Trigger.Acknowledge, ImportNotificationStatus.DecisionRequiredBy)
                .Permit(Trigger.Withdraw, ImportNotificationStatus.Withdrawn)
                .Permit(Trigger.Object, ImportNotificationStatus.Objected)
                .Permit(Trigger.FileClosed, ImportNotificationStatus.FileClosed);

            stateMachine.Configure(ImportNotificationStatus.DecisionRequiredBy)
                .OnEntryFrom(acknowledgeTrigger, OnAcknowledged)
                .Permit(Trigger.Consent, ImportNotificationStatus.Consented)
                .Permit(Trigger.Object, ImportNotificationStatus.Objected)
                .Permit(Trigger.FileClosed, ImportNotificationStatus.FileClosed);

            stateMachine.Configure(ImportNotificationStatus.Consented)
                .OnEntryFrom(consentedTrigger, OnConsented)
                .Permit(Trigger.WithdrawConsent, ImportNotificationStatus.ConsentWithdrawn)
                .Permit(Trigger.FileClosed, ImportNotificationStatus.FileClosed);

            stateMachine.Configure(ImportNotificationStatus.ConsentWithdrawn)
                .OnEntryFrom(withdrawConsentTrigger, OnConsentWithdrawn)
                .Permit(Trigger.FileClosed, ImportNotificationStatus.FileClosed);

            stateMachine.Configure(ImportNotificationStatus.Objected)
                .Permit(Trigger.FileClosed, ImportNotificationStatus.FileClosed);

            stateMachine.Configure(ImportNotificationStatus.Withdrawn)
                .Permit(Trigger.FileClosed, ImportNotificationStatus.FileClosed);

            stateMachine.Configure(ImportNotificationStatus.FileClosed)
                .OnEntryFrom(fileClosedTrigger, OnFileClosed);

            return stateMachine;
        }

        private void OnConsented(DateTime consentDate)
        {
            Dates.ConsentedDate = consentDate;
        }

        private void OnConsentWithdrawn(DateTime withdrawnDate, string reasons)
        {
            Dates.ConsentWithdrawnDate = withdrawnDate;
            Dates.ConsentWithdrawnReasons = reasons;
        }

        private void OnAcknowledged(DateTime acknowledgedDate)
        {
            Dates.AcknowledgedDate = acknowledgedDate;
        }

        private void OnNotificationCompleted(DateTime completedDate)
        {
            Dates.NotificationCompletedDate = completedDate;
        }

        private void OnAssessmentStarted(DateTime date, string officer)
        {
            Dates.AssessmentStartedDate = date;
            Dates.NameOfOfficer = officer;
        }

        private void OnFullyPaid(DateTime paymentDate)
        {
            Dates.PaymentReceivedDate = paymentDate;
        }

        private void OnTransition(StateMachine<ImportNotificationStatus, Trigger>.Transition transition)
        {
            RaiseEvent(new ImportNotificationStatusChangeEvent(this, transition.Source, transition.Destination));
        }

        public void AddStatusChangeRecord(ImportNotificationStatusChange statusChange)
        {
            Guard.ArgumentNotNull(() => statusChange, statusChange);

            if (StatusChangeCollection == null)
            {
                StatusChangeCollection = new List<ImportNotificationStatusChange>();
            }

            StatusChangeCollection.Add(statusChange);
        }

        public void Receive(DateTime receivedDate)
        {
            stateMachine.Fire(receivedTrigger, receivedDate);
        }

        private void OnReceived(DateTime receivedDate)
        {
            Dates.NotificationReceivedDate = receivedDate;
        }

        private void OnFileClosed(DateTime fileClosedDate)
        {
            Dates.FileClosedDate = fileClosedDate;
        }

        public void Submit()
        {
            stateMachine.Fire(Trigger.Submit);

            RaiseEvent(new ImportNotificationSubmittedEvent(this));
        }

        public void PaymentComplete(DateTime date)
        {
            stateMachine.Fire(fullyPaidTrigger, date);
        }

        public void BeginAssessment(DateTime date, string officer)
        {
            stateMachine.Fire(beginAssessmentTrigger, date, officer);
        }

        public void CompleteNotification(DateTime date)
        {
            stateMachine.Fire(completeNotificationTrigger, date);
        }

        public void Acknowledge(DateTime date)
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

        internal ImportConsent Consent(DateRange dateRange,
            string conditions,
            Guid userId,
            DateTime consentedDate)
        {
            stateMachine.Fire(consentedTrigger, consentedDate);

            return new ImportConsent(NotificationApplicationId, dateRange, conditions, userId);
        }

        internal ImportWithdrawn Withdraw(DateTime date, string reasons)
        {
            stateMachine.Fire(Trigger.Withdraw);

            return new ImportWithdrawn(NotificationApplicationId, date, reasons);
        }

        internal ImportObjection Object(DateTime date, string reasons)
        {
            stateMachine.Fire(Trigger.Object);

            return new ImportObjection(NotificationApplicationId, date, reasons);
        }

        public void MarkFileClosed(DateTime fileClosedDate)
        {
            stateMachine.Fire(fileClosedTrigger, fileClosedDate);
        }

        public void SetArchiveReference(string reference)
        {
            if (!Dates.FileClosedDate.HasValue)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Can't set archive reference for notification {0} if it is not marked as file closed",
                        NotificationApplicationId));
            }
            Dates.ArchiveReference = reference;
        }
    }
}
