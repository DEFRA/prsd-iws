namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Admin;
    using Core.NotificationAssessment;
    using Core.Shared;
    using NotificationApplication;
    using NotificationConsent;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using Stateless;

    public class NotificationAssessment : Entity
    {
        private enum Trigger
        {
            Submit,
            NotificationReceived,
            AssessmentCommenced,
            NotificationComplete,
            Transmit,
            Acknowledged,
            DecisionRequiredBySet,
            Withdraw,
            Object,
            Consent,
            WithdrawConsent,
            Unlock,
            Resubmit,
            AcceptChanges,
            RejectChanges
        }

        private static readonly BidirectionalDictionary<DecisionType, Trigger> DecisionTriggers
            = new BidirectionalDictionary<DecisionType, Trigger>(new Dictionary<DecisionType, Trigger>
                {
                    { DecisionType.Consent, Trigger.Consent },
                    { DecisionType.Withdraw, Trigger.Withdraw },
                    { DecisionType.Object, Trigger.Object },
                    { DecisionType.ConsentWithdraw, Trigger.WithdrawConsent }
                });

        private readonly StateMachine<NotificationStatus, Trigger> stateMachine;

        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> receivedTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime, string> commencedTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> completeTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> transmitTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> acknowledgedTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime, string> withdrawTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime, string> objectTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime, string> withdrawConsentTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> consentedTrigger;  

        public Guid NotificationApplicationId { get; private set; }

        public NotificationStatus Status { get; private set; }
        
        protected virtual ICollection<NotificationStatusChange> StatusChangeCollection { get; set; }
        
        public IEnumerable<NotificationStatusChange> StatusChanges
        {
            get { return StatusChangeCollection.ToSafeIEnumerable(); }
        }

        public virtual NotificationDates Dates { get; set; }

        public bool CanEditNotification
        {
            get
            {
                return stateMachine.IsInState(NotificationStatus.NotSubmitted)
                       || stateMachine.IsInState(NotificationStatus.Submitted)
                       || stateMachine.IsInState(NotificationStatus.NotificationReceived)
                       || stateMachine.IsInState(NotificationStatus.InAssessment)
                       || stateMachine.IsInState(NotificationStatus.Unlocked);
            }
        }

        protected NotificationAssessment()
        {
            stateMachine = CreateStateMachine();
        }

        public NotificationAssessment(Guid notificationApplicationId)
        {
            NotificationApplicationId = notificationApplicationId;
            Status = NotificationStatus.NotSubmitted;
            StatusChangeCollection = new List<NotificationStatusChange>();
            stateMachine = CreateStateMachine();
            Dates = new NotificationDates();
        }

        private StateMachine<NotificationStatus, Trigger> CreateStateMachine()
        {
            var stateMachine = new StateMachine<NotificationStatus, Trigger>(() => Status, s => Status = s);

            receivedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.NotificationReceived);
            commencedTrigger = stateMachine.SetTriggerParameters<DateTime, string>(Trigger.AssessmentCommenced);
            completeTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.NotificationComplete);
            transmitTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Transmit);
            acknowledgedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Acknowledged);
            withdrawTrigger = stateMachine.SetTriggerParameters<DateTime, string>(Trigger.Withdraw);
            objectTrigger = stateMachine.SetTriggerParameters<DateTime, string>(Trigger.Object);
            withdrawConsentTrigger = stateMachine.SetTriggerParameters<DateTime, string>(Trigger.WithdrawConsent);
            consentedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Consent);

            stateMachine.OnTransitioned(OnTransitionAction);

            stateMachine.Configure(NotificationStatus.NotSubmitted)
                .Permit(Trigger.Submit, NotificationStatus.Submitted);

            stateMachine.Configure(NotificationStatus.Submitted)
                .SubstateOf(NotificationStatus.InDetermination)
                .OnEntryFrom(Trigger.Submit, OnSubmit)
                .Permit(Trigger.NotificationReceived, NotificationStatus.NotificationReceived);

            stateMachine.Configure(NotificationStatus.NotificationReceived)
                .SubstateOf(NotificationStatus.InDetermination)
                .OnEntryFrom(receivedTrigger, OnReceived)
                .PermitIf(Trigger.AssessmentCommenced, NotificationStatus.InAssessment, () => Dates.PaymentReceivedDate.HasValue)
                .Permit(Trigger.Object, NotificationStatus.Objected);

            stateMachine.Configure(NotificationStatus.InAssessment)
                .SubstateOf(NotificationStatus.InDetermination)
                .OnEntryFrom(commencedTrigger, OnInAssessment)
                .Permit(Trigger.NotificationComplete, NotificationStatus.ReadyToTransmit)
                .Permit(Trigger.Object, NotificationStatus.Objected);

            stateMachine.Configure(NotificationStatus.ReadyToTransmit)
                .SubstateOf(NotificationStatus.InDetermination)
                .OnEntryFrom(completeTrigger, OnCompleted)
                .Permit(Trigger.Transmit, NotificationStatus.Transmitted)
                .Permit(Trigger.Object, NotificationStatus.Objected);

            stateMachine.Configure(NotificationStatus.Transmitted)
                .SubstateOf(NotificationStatus.InDetermination)
                .OnEntryFrom(transmitTrigger, OnTransmitted)
                .Permit(Trigger.Acknowledged, NotificationStatus.DecisionRequiredBy)
                .Permit(Trigger.Object, NotificationStatus.Objected);

            stateMachine.Configure(NotificationStatus.DecisionRequiredBy)
                .OnEntryFrom(acknowledgedTrigger, OnAcknowledged)
                .Permit(Trigger.Unlock, NotificationStatus.Unlocked)
                .Permit(Trigger.Consent, NotificationStatus.Consented)
                .Permit(Trigger.Object, NotificationStatus.Objected);

            stateMachine.Configure(NotificationStatus.InDetermination)
                .Permit(Trigger.Withdraw, NotificationStatus.Withdrawn);

            stateMachine.Configure(NotificationStatus.Withdrawn)
                .OnEntryFrom(withdrawTrigger, OnWithdrawn);

            stateMachine.Configure(NotificationStatus.Objected)
                .OnEntryFrom(objectTrigger, OnObjected);

            stateMachine.Configure(NotificationStatus.Consented)
                .OnEntryFrom(consentedTrigger, OnConsented)
                .Permit(Trigger.WithdrawConsent, NotificationStatus.ConsentWithdrawn);

            stateMachine.Configure(NotificationStatus.ConsentWithdrawn)
                .OnEntryFrom(withdrawConsentTrigger, OnConsentWithdrawn);

            stateMachine.Configure(NotificationStatus.Unlocked)
                .SubstateOf(NotificationStatus.InDetermination)
                .Permit(Trigger.Resubmit, NotificationStatus.Reassessment);

            stateMachine.Configure(NotificationStatus.Reassessment)
                .SubstateOf(NotificationStatus.InDetermination)
                .Permit(Trigger.AcceptChanges, NotificationStatus.DecisionRequiredBy)
                .Permit(Trigger.RejectChanges, NotificationStatus.Unlocked);

            return stateMachine;
        }

        private void OnConsented(DateTime consentedDate)
        {
            Dates.ConsentedDate = consentedDate;
        }

        private void OnConsentWithdrawn(DateTime withdrawnDate, string reasons)
        {
            Dates.ConsentWithdrawnDate = withdrawnDate;
            Dates.ConsentWithdrawnReasons = reasons;
        }

        private void OnWithdrawn(DateTime withdrawnDate, string reason)
        {
            Dates.WithdrawnDate = withdrawnDate;
            Dates.WithdrawnReason = reason;
        }

        private void OnSubmit()
        {
            RaiseEvent(new NotificationSubmittedEvent(NotificationApplicationId));
        }

        private void OnReceived(DateTime receivedDate)
        {
            Dates.NotificationReceivedDate = receivedDate;
        }

        private void OnInAssessment(DateTime commencementDate, string nameOfOfficer)
        {
            Dates.CommencementDate = commencementDate;
            Dates.NameOfOfficer = nameOfOfficer;
        }

        private void OnCompleted(DateTime completedDate)
        {
            Dates.CompleteDate = completedDate;
        }

        private void OnTransmitted(DateTime transmittedDate)
        {
            Dates.TransmittedDate = transmittedDate;
            RaiseEvent(new NotificationTransmittedEvent(NotificationApplicationId));
        }

        private void OnObjected(DateTime objectionDate, string reason)
        {
            Dates.ObjectedDate = objectionDate;
            Dates.ObjectionReason = reason;
        }

        private void OnTransitionAction(StateMachine<NotificationStatus, Trigger>.Transition transition)
        {
            RaiseEvent(new NotificationStatusChangeEvent(this, transition.Destination));
        }

        private void OnAcknowledged(DateTime acknowledgedDate)
        {
            Dates.AcknowledgedDate = acknowledgedDate;
        }

        public void Submit(INotificationProgressService progressService)
        {
            if (!progressService.IsComplete(NotificationApplicationId))
            {
                throw new InvalidOperationException(string.Format("Cannot submit an incomplete notification: {0}", NotificationApplicationId));
            }

            stateMachine.Fire(Trigger.Submit);
        }

        public void AddStatusChangeRecord(NotificationStatusChange statusChange)
        {
            Guard.ArgumentNotNull(() => statusChange, statusChange);

            StatusChangeCollection.Add(statusChange);
        }

        public void NotificationReceived(DateTime receivedDate)
        {
            stateMachine.Fire(receivedTrigger, receivedDate);
        }

        public void Commenced(DateTime commencementDate, string nameOfOfficer)
        {
            Guard.ArgumentNotNullOrEmpty(() => nameOfOfficer, nameOfOfficer);
            stateMachine.Fire(commencedTrigger, commencementDate, nameOfOfficer);
        }

        internal void Complete(DateTime completedDate)
        {
            stateMachine.Fire(completeTrigger, completedDate);
        }

        public void Transmit(DateTime transmittedDate)
        {
            stateMachine.Fire(transmitTrigger, transmittedDate);
        }

        public void Acknowledge(DateTime acknowledgedDate)
        {
            stateMachine.Fire(acknowledgedTrigger, acknowledgedDate);
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

        internal Consent Consent(DateRange dateRange, string conditions, Guid userId, DateTime consentedDate)
        {
            stateMachine.Fire(consentedTrigger, consentedDate);

            return new Consent(NotificationApplicationId, dateRange, conditions, userId);
        }

        public void Withdraw(DateTime withdrawnDate, string reason)
        {
            stateMachine.Fire(withdrawTrigger, withdrawnDate, reason);
        }

        public void Object(DateTime objectionDate, string reason)
        {
            stateMachine.Fire(objectTrigger, objectionDate, reason);
        }

        public void Unlock()
        {
            stateMachine.Fire(Trigger.Unlock);
        }

        public void Resubmit()
        {
            stateMachine.Fire(Trigger.Resubmit);
        }

        public void AcceptChanges()
        {
            stateMachine.Fire(Trigger.AcceptChanges);
        }

        public void RejectChanges()
        {
            stateMachine.Fire(Trigger.RejectChanges);
        }
    }
}