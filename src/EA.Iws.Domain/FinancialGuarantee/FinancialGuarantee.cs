namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;
    using System.Collections.Generic;
    using Core.FinancialGuarantee;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using Stateless;

    public class FinancialGuarantee : Entity
    {
        private const int WorkingDaysUntilDecisionRequired = 20;
        
        public Guid NotificationApplicationId { get; protected set; }

        private StateMachine<FinancialGuaranteeStatus, Trigger>.TriggerWithParameters<DateTime> receivedTrigger;
        private StateMachine<FinancialGuaranteeStatus, Trigger>.TriggerWithParameters<DateTime> completedTrigger;
        private StateMachine<FinancialGuaranteeStatus, Trigger>.TriggerWithParameters<ApproveDates> approvedTrigger;
        private StateMachine<FinancialGuaranteeStatus, Trigger>.TriggerWithParameters<DateTime, string> refusedTrigger;
        private StateMachine<FinancialGuaranteeStatus, Trigger>.TriggerWithParameters<DateTime> releasedTrigger;
        private readonly StateMachine<FinancialGuaranteeStatus, Trigger> stateMachine;

        protected FinancialGuarantee()
        {
            stateMachine = CreateStateMachine();
        }

        protected FinancialGuarantee(Guid notificationApplicationId)
        {
            CreatedDate = SystemTime.UtcNow;
            NotificationApplicationId = notificationApplicationId;
            StatusChangeCollection = new List<FinancialGuaranteeStatusChange>();
            stateMachine = CreateStateMachine();
            Status = FinancialGuaranteeStatus.AwaitingApplication;
        }

        public DateTime? ReceivedDate { get; private set; }

        public DateTime? CompletedDate { get; private set; }

        public DateTime? GetDecisionRequiredDate(IWorkingDayCalculator workingDayCalculator, UKCompetentAuthority competentAuthority)
        {
            DateTime? returnDate = null;

            if (CompletedDate.HasValue)
            {
                returnDate = workingDayCalculator.AddWorkingDays(CompletedDate.Value, WorkingDaysUntilDecisionRequired, false);
            }

            return returnDate;
        }

        public DateTime CreatedDate { get; private set; }

        public FinancialGuaranteeStatus Status { get; protected set; }

        protected virtual ICollection<FinancialGuaranteeStatusChange> StatusChangeCollection { get; set; }

        public IEnumerable<FinancialGuaranteeStatusChange> StatusChanges
        {
            get { return StatusChangeCollection.ToSafeIEnumerable(); }
        }

        public DateTime? DecisionDate { get; protected set; }

        public DateTime? ApprovedFrom { get; protected set; }

        public DateTime? ApprovedTo { get; protected set; }

        public string RefusalReason { get; protected set; }

        public int? ActiveLoadsPermitted { get; protected set; }

        public void Received(DateTime date)
        {
            stateMachine.Fire(receivedTrigger, date);
        }

        public void Completed(DateTime date)
        {
            stateMachine.Fire(completedTrigger, date);
        }

        public void AddStatusChangeRecord(FinancialGuaranteeStatusChange statusChange)
        {
            Guard.ArgumentNotNull(() => statusChange, statusChange);

            StatusChangeCollection.Add(statusChange);
        }

        public static FinancialGuarantee Create(Guid notificationApplicationId)
        {
            var financialGuarantee = new FinancialGuarantee(notificationApplicationId);

            return financialGuarantee;
        }

        private void OnReceived(DateTime date)
        {
            ReceivedDate = date;
        }

        private void OnCompleted(DateTime date)
        {
            if (date < ReceivedDate.Value)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot set FG completed date before received date for financial guarantee {0}.", Id));
            }

            CompletedDate = date;
        }

        private StateMachine<FinancialGuaranteeStatus, Trigger> CreateStateMachine()
        {
            var stateMachine = new StateMachine<FinancialGuaranteeStatus, Trigger>(() => Status, s => Status = s);

            receivedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Received);
            completedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Completed);
            approvedTrigger = stateMachine.SetTriggerParameters<ApproveDates>(Trigger.Approved);
            refusedTrigger = stateMachine.SetTriggerParameters<DateTime, string>(Trigger.Refused);
            releasedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Released);

            stateMachine.Configure(FinancialGuaranteeStatus.AwaitingApplication)
                .Permit(Trigger.Received, FinancialGuaranteeStatus.ApplicationReceived);

            stateMachine.Configure(FinancialGuaranteeStatus.ApplicationReceived)
                .OnEntryFrom(receivedTrigger, OnReceived)
                .Permit(Trigger.Completed, FinancialGuaranteeStatus.ApplicationComplete);

            stateMachine.Configure(FinancialGuaranteeStatus.ApplicationComplete)
                .OnEntryFrom(completedTrigger, OnCompleted)
                .PermitIf(Trigger.Approved, FinancialGuaranteeStatus.Approved, () => CompletedDate.HasValue)
                .PermitIf(Trigger.Refused, FinancialGuaranteeStatus.Refused, () => CompletedDate.HasValue)
                .PermitIf(Trigger.Released, FinancialGuaranteeStatus.Released, () => CompletedDate.HasValue);

            stateMachine.Configure(FinancialGuaranteeStatus.Approved)
                .OnEntryFrom(approvedTrigger, OnApproved)
                .Permit(Trigger.Released, FinancialGuaranteeStatus.Released);

            stateMachine.Configure(FinancialGuaranteeStatus.Refused)
                .OnEntryFrom(refusedTrigger, OnRefused)
                .Permit(Trigger.Released, FinancialGuaranteeStatus.Released);

            stateMachine.Configure(FinancialGuaranteeStatus.Released)
                .OnEntryFrom(releasedTrigger, OnReleased);

            stateMachine.OnTransitioned(OnTransitionAction);

            return stateMachine;
        }

        private void OnTransitionAction(StateMachine<FinancialGuaranteeStatus, Trigger>.Transition transition)
        {
            RaiseEvent(new FinancialGuaranteeStatusChangeEvent(this, transition.Destination));
        }

        private enum Trigger
        {
            Received = 0,
            Completed = 1,
            Approved = 2,
            Refused,
            Released
        }

        public void UpdateReceivedDate(DateTime value)
        {
            if (ReceivedDate.HasValue && (!CompletedDate.HasValue || value <= CompletedDate))
            {
                ReceivedDate = value;
            }
            else
            {
                throw new InvalidOperationException("Received date must have value and value to set must be less than received date.");
            }
        }

        public void UpdateCompletedDate(DateTime value)
        {
            if (CompletedDate.HasValue && ReceivedDate.HasValue && value >= ReceivedDate)
            {
                CompletedDate = value;
            }
            else
            {
                throw new InvalidOperationException("Completed date must have value and value to set must be greater than received date.");
            }
        }

        public virtual void Approve(ApproveDates approveDates)
        {
            if (approveDates.DecisionDate < CompletedDate)
            {
                throw new InvalidOperationException("Cannot set the decision date to before the completed date. Id: " +
                                                    NotificationApplicationId);
            }

            stateMachine.Fire(approvedTrigger, approveDates);
        }

        private void OnApproved(ApproveDates approveDates)
        {
            DecisionDate = approveDates.DecisionDate;
            ApprovedTo = approveDates.ApprovedTo;
            ApprovedFrom = approveDates.ApprovedFrom;
            ActiveLoadsPermitted = approveDates.ActiveLoadsPermitted;
        }

        public virtual void Refuse(DateTime decisionDate, string refusalReason)
        {
            Guard.ArgumentNotNullOrEmpty(() => refusalReason, refusalReason);

            if (string.IsNullOrWhiteSpace(refusalReason))
            {
                throw new ArgumentException("Refusal reason cannot be whitespace. Id: " + NotificationApplicationId);
            }

            if (decisionDate < CompletedDate)
            {
                throw new InvalidOperationException("Cannot set the decision date to be before the completed date. Id: " + NotificationApplicationId);
            }

            stateMachine.Fire(refusedTrigger, decisionDate, refusalReason);
        }

        private void OnRefused(DateTime decisionDate, string refusalReason)
        {
            DecisionDate = decisionDate;
            RefusalReason = refusalReason;
        }

        public virtual void Release(DateTime decisionDate)
        {
            stateMachine.Fire(releasedTrigger, decisionDate);
        }

        private void OnReleased(DateTime decisionDate)
        {
            if (decisionDate < CompletedDate)
            {
                throw new InvalidOperationException("Cannot set the decision date to be before the completed date. Id: " + NotificationApplicationId);
            }

            DecisionDate = decisionDate;
        }
    }
}