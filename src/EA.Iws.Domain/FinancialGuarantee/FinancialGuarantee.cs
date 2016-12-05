namespace EA.Iws.Domain.FinancialGuarantee
{
    using System;
    using System.Collections.Generic;
    using Core.FinancialGuarantee;
    using Core.Notification;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using Stateless;

    public class FinancialGuarantee : Entity
    {
        private const int WorkingDaysUntilDecisionRequired = 20;

        private StateMachine<FinancialGuaranteeStatus, Trigger>.TriggerWithParameters<DateTime> completedTrigger;
        private StateMachine<FinancialGuaranteeStatus, Trigger>.TriggerWithParameters<ApprovalData> approvedTrigger;
        private StateMachine<FinancialGuaranteeStatus, Trigger>.TriggerWithParameters<DateTime, string> refusedTrigger;
        private StateMachine<FinancialGuaranteeStatus, Trigger>.TriggerWithParameters<DateTime> releasedTrigger;
        private readonly StateMachine<FinancialGuaranteeStatus, Trigger> stateMachine;

        protected FinancialGuarantee()
        {
            stateMachine = CreateStateMachine();
        }

        internal FinancialGuarantee(DateTime receivedDate)
        {
            CreatedDate = new DateTimeOffset(SystemTime.UtcNow, TimeSpan.Zero);
            ReceivedDate = receivedDate;
            StatusChangeCollection = new List<FinancialGuaranteeStatusChange>();
            stateMachine = CreateStateMachine();
            Status = FinancialGuaranteeStatus.ApplicationReceived;
            Decision = FinancialGuaranteeDecision.None;
        }

        public DateTime ReceivedDate { get; private set; }

        public DateTime? CompletedDate { get; private set; }

        public DateTime? GetDecisionRequiredDate(IWorkingDayCalculator workingDayCalculator, UKCompetentAuthority competentAuthority)
        {
            DateTime? returnDate = null;

            if (CompletedDate.HasValue)
            {
                returnDate = workingDayCalculator.AddWorkingDays(CompletedDate.Value, WorkingDaysUntilDecisionRequired, false, competentAuthority);
            }

            return returnDate;
        }

        public DateTimeOffset CreatedDate { get; private set; }

        public FinancialGuaranteeStatus Status { get; protected set; }

        public FinancialGuaranteeDecision Decision { get; private set; }

        protected virtual ICollection<FinancialGuaranteeStatusChange> StatusChangeCollection { get; set; }

        public IEnumerable<FinancialGuaranteeStatusChange> StatusChanges
        {
            get { return StatusChangeCollection.ToSafeIEnumerable(); }
        }

        public DateTime? DecisionDate { get; protected set; }

        public string RefusalReason { get; protected set; }

        public string ReferenceNumber { get; protected set; }

        public int? ActiveLoadsPermitted { get; protected set; }

        public DateTime? ReleasedDate { get; private set; }

        public bool? IsBlanketBond { get; private set; }

        public void Complete(DateTime date)
        {
            if (date < ReceivedDate)
            {
                throw new InvalidOperationException(
                    string.Format("Cannot set FG completed date before received date for financial guarantee {0}.", Id));
            }

            stateMachine.Fire(completedTrigger, date);
        }

        public void AddStatusChangeRecord(FinancialGuaranteeStatusChange statusChange)
        {
            Guard.ArgumentNotNull(() => statusChange, statusChange);

            StatusChangeCollection.Add(statusChange);
        }

        private void OnCompleted(DateTime date)
        {
            CompletedDate = date;
        }

        private StateMachine<FinancialGuaranteeStatus, Trigger> CreateStateMachine()
        {
            var stateMachine = new StateMachine<FinancialGuaranteeStatus, Trigger>(() => Status, s => Status = s);

            completedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Completed);
            approvedTrigger = stateMachine.SetTriggerParameters<ApprovalData>(Trigger.Approved);
            refusedTrigger = stateMachine.SetTriggerParameters<DateTime, string>(Trigger.Refused);
            releasedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Released);

            stateMachine.Configure(FinancialGuaranteeStatus.ApplicationReceived)
                .Permit(Trigger.Completed, FinancialGuaranteeStatus.ApplicationComplete);

            stateMachine.Configure(FinancialGuaranteeStatus.ApplicationComplete)
                .OnEntryFrom(completedTrigger, OnCompleted)
                .PermitIf(Trigger.Approved, FinancialGuaranteeStatus.Approved, () => CompletedDate.HasValue)
                .PermitIf(Trigger.Refused, FinancialGuaranteeStatus.Refused, () => CompletedDate.HasValue)
                .PermitIf(Trigger.Released, FinancialGuaranteeStatus.Released, () => CompletedDate.HasValue);

            stateMachine.Configure(FinancialGuaranteeStatus.Approved)
                .OnEntryFrom(approvedTrigger, OnApproved)
                .Permit(Trigger.Released, FinancialGuaranteeStatus.Released)
                .Permit(Trigger.Superseded, FinancialGuaranteeStatus.Superseded);

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
            Completed,
            Approved,
            Refused,
            Released,
            Superseded
        }

        internal void Approve(ApprovalData approvalData)
        {
            if (approvalData.DecisionDate < CompletedDate)
            {
                throw new InvalidOperationException("Cannot set the decision date to before the completed date. Id: " + Id);
            }

            stateMachine.Fire(approvedTrigger, approvalData);
        }

        private void OnApproved(ApprovalData approvalData)
        {
            DecisionDate = approvalData.DecisionDate;
            Decision = FinancialGuaranteeDecision.Approved;
            ActiveLoadsPermitted = approvalData.ActiveLoadsPermitted;
            ReferenceNumber = approvalData.ReferenceNumber;
            IsBlanketBond = approvalData.IsBlanketBond;
        }

        public virtual void Refuse(DateTime decisionDate, string refusalReason)
        {
            Guard.ArgumentNotNullOrEmpty(() => refusalReason, refusalReason);

            if (string.IsNullOrWhiteSpace(refusalReason))
            {
                throw new ArgumentException("Refusal reason cannot be white space. Id: " + Id);
            }

            if (decisionDate < CompletedDate)
            {
                throw new InvalidOperationException("Cannot set the decision date to be before the completed date. Id: " + Id);
            }

            stateMachine.Fire(refusedTrigger, decisionDate, refusalReason);
        }

        private void OnRefused(DateTime decisionDate, string refusalReason)
        {
            DecisionDate = decisionDate;
            Decision = FinancialGuaranteeDecision.Refused;
            RefusalReason = refusalReason;
        }

        public virtual void Release(DateTime releasedDate)
        {
            if (releasedDate < CompletedDate)
            {
                throw new InvalidOperationException("Cannot set the released date to be before the completed date. Id: " + Id);
            }

            stateMachine.Fire(releasedTrigger, releasedDate);
        }

        private void OnReleased(DateTime releasedDate)
        {
            ReleasedDate = releasedDate;

            if (Decision == FinancialGuaranteeDecision.None)
            {
                Decision = FinancialGuaranteeDecision.Released;
                DecisionDate = releasedDate;
            }
        }

        internal void Supersede()
        {
            stateMachine.Fire(Trigger.Superseded);
        }
    }
}