namespace EA.Iws.Domain.NotificationAssessment
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Core.Admin;
    using Core.NotificationAssessment;
    using NotificationApplication;
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
            WithdrawConsent
        }

        private static readonly IDictionary<DecisionType, Trigger> DecisionTriggers = new Dictionary<DecisionType, Trigger>
        {
            { DecisionType.Consent, Trigger.Consent },
            { DecisionType.Withdrawn, Trigger.Withdraw },
            { DecisionType.Object, Trigger.Object },
            { DecisionType.ConsentWithdrawn, Trigger.WithdrawConsent }
        }; 

        private readonly StateMachine<NotificationStatus, Trigger> stateMachine;

        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> receivedTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime, string> commencedTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> completeTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> transmitTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> acknowledgedTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<DateTime> decisionRequiredByTrigger;
        private StateMachine<NotificationStatus, Trigger>.TriggerWithParameters<ConsentDecision> consentTrigger;

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
                       || stateMachine.IsInState(NotificationStatus.InAssessment);
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
            decisionRequiredByTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.DecisionRequiredBySet);
            consentTrigger = stateMachine.SetTriggerParameters<ConsentDecision>(Trigger.Consent);

            stateMachine.OnTransitioned(OnTransitionAction);

            stateMachine.Configure(NotificationStatus.NotSubmitted)
                .Permit(Trigger.Submit, NotificationStatus.Submitted);

            stateMachine.Configure(NotificationStatus.Submitted)
                .OnEntryFrom(Trigger.Submit, OnSubmit)
                .Permit(Trigger.NotificationReceived, NotificationStatus.NotificationReceived)
                .Permit(Trigger.Withdraw, NotificationStatus.Withdrawn)
                .Permit(Trigger.Object, NotificationStatus.Objected);

            stateMachine.Configure(NotificationStatus.NotificationReceived)
                .OnEntryFrom(receivedTrigger, OnReceived)
                .PermitIf(Trigger.AssessmentCommenced, NotificationStatus.InAssessment, () => Dates.PaymentReceivedDate.HasValue)
                .Permit(Trigger.Withdraw, NotificationStatus.Withdrawn)
                .Permit(Trigger.Object, NotificationStatus.Objected);

            stateMachine.Configure(NotificationStatus.InAssessment)
                .OnEntryFrom(commencedTrigger, OnInAssessment)
                .Permit(Trigger.NotificationComplete, NotificationStatus.ReadyToTransmit)
                .Permit(Trigger.Withdraw, NotificationStatus.Withdrawn)
                .Permit(Trigger.Object, NotificationStatus.Objected);

            stateMachine.Configure(NotificationStatus.ReadyToTransmit)
                .OnEntryFrom(completeTrigger, OnCompleted)
                .Permit(Trigger.Transmit, NotificationStatus.Transmitted)
                .Permit(Trigger.Withdraw, NotificationStatus.Withdrawn)
                .Permit(Trigger.Object, NotificationStatus.Objected);

            stateMachine.Configure(NotificationStatus.Transmitted)
                .OnEntryFrom(transmitTrigger, OnTransmitted)
                .Permit(Trigger.Acknowledged, NotificationStatus.Acknowledged)
                .Permit(Trigger.Withdraw, NotificationStatus.Withdrawn)
                .Permit(Trigger.Object, NotificationStatus.Objected);

            stateMachine.Configure(NotificationStatus.Acknowledged)
                .OnEntryFrom(acknowledgedTrigger, OnAcknowledged)
                .Permit(Trigger.DecisionRequiredBySet, NotificationStatus.DecisionRequiredBy)
                .Permit(Trigger.Withdraw, NotificationStatus.Withdrawn)
                .Permit(Trigger.Object, NotificationStatus.Objected);

            stateMachine.Configure(NotificationStatus.DecisionRequiredBy)
                .OnEntryFrom(decisionRequiredByTrigger, OnDecisionRequiredBy)
                .Permit(Trigger.Consent, NotificationStatus.Consented)
                .Permit(Trigger.Withdraw, NotificationStatus.Withdrawn)
                .Permit(Trigger.Object, NotificationStatus.Objected);

            stateMachine.Configure(NotificationStatus.Consented)
                .OnEntryFrom(consentTrigger, OnConsented)
                .Permit(Trigger.WithdrawConsent, NotificationStatus.ConsentWithdrawn);

            return stateMachine;
        }

        private void OnSubmit()
        {
            RaiseEvent(new NotificationSubmittedEvent(NotificationApplicationId));
        }

        private void OnConsented(ConsentDecision decision)
        {
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
        }

        private void OnTransitionAction(StateMachine<NotificationStatus, Trigger>.Transition transition)
        {
            RaiseEvent(new NotificationStatusChangeEvent(this, transition.Destination));
        }

        private void OnAcknowledged(DateTime acknowledgedDate)
        {
            Dates.AcknowledgedDate = acknowledgedDate;
        }

        private void OnDecisionRequiredBy(DateTime decisionByDate)
        {
            Dates.DecisionDate = decisionByDate;
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

        public void PaymentReceived(DateTime paymentDate)
        {
            if (!stateMachine.IsInState(NotificationStatus.NotificationReceived))
            {
                throw new InvalidOperationException(string.Format("Cannot set payment received on this notification {0} in status {1}", NotificationApplicationId, Status));
            }

            Dates.PaymentReceivedDate = paymentDate;
        }

        public void Commenced(DateTime commencementDate, string nameOfOfficer)
        {
            Guard.ArgumentNotNullOrEmpty(() => nameOfOfficer, nameOfOfficer);
            stateMachine.Fire(commencedTrigger, commencementDate, nameOfOfficer);
        }

        public void Complete(DateTime completedDate)
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

        public void DecisionRequiredBy(DateTime decisionRequiredByDate)
        {
            stateMachine.Fire(decisionRequiredByTrigger, decisionRequiredByDate);
        }

        public IEnumerable<DecisionType> GetAvailableDecisions()
        {
            var triggers = stateMachine.PermittedTriggers.Where(t =>
            {
                return DecisionTriggers.Any(dt => dt.Value == t);
            });

            return DecisionTriggers.Where(dt => triggers.Contains(dt.Value)).Select(dt => dt.Key);
        }
    }
}