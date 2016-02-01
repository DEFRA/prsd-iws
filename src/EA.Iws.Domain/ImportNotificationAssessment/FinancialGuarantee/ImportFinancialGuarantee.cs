namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using System.Collections.Generic;
    using Core.FinancialGuarantee;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using Stateless;

    public class ImportFinancialGuarantee : Entity
    {
        private readonly StateMachine<FinancialGuaranteeStatus, Trigger> stateMachine;

        private StateMachine<FinancialGuaranteeStatus, Trigger>.TriggerWithParameters<DateTime> completeTrigger;  

        public Guid ImportNotificationId { get; private set; }

        public FinancialGuaranteeStatus Status { get; private set; }

        public DateTime ReceivedDate { get; private set; }

        public DateTime? CompletedDate { get; private set; }

        public DateTimeOffset CreatedDate { get; private set; }

        protected virtual ICollection<ImportFinancialGuaranteeStatusChange> StatusChangeCollection { get; set; }

        public IEnumerable<ImportFinancialGuaranteeStatusChange> StatusChanges
        {
            get { return StatusChangeCollection.ToSafeIEnumerable(); }
        } 

        protected ImportFinancialGuarantee()
        {
            stateMachine = CreateStateMachine();
            StatusChangeCollection = new List<ImportFinancialGuaranteeStatusChange>();
        }

        public ImportFinancialGuarantee(Guid importNotificationId, DateTime receivedDate) : this()
        {
            ImportNotificationId = importNotificationId;
            ReceivedDate = receivedDate;
            CreatedDate = DateTimeOffset.UtcNow;
            Status = FinancialGuaranteeStatus.ApplicationReceived;
        }

        private StateMachine<FinancialGuaranteeStatus, Trigger> CreateStateMachine()
        {
            var stateMachine = new StateMachine<FinancialGuaranteeStatus, Trigger>(() => Status, s => Status = s);

            stateMachine.OnTransitioned(OnTransitionAction);

            completeTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Complete);

            stateMachine.Configure(FinancialGuaranteeStatus.ApplicationReceived)
                .Permit(Trigger.Complete, FinancialGuaranteeStatus.ApplicationComplete);

            stateMachine.Configure(FinancialGuaranteeStatus.ApplicationComplete)
                .OnEntryFrom(completeTrigger, OnComplete);

            return stateMachine;
        }

        private void OnTransitionAction(StateMachine<FinancialGuaranteeStatus, Trigger>.Transition transition)
        {
            RaiseEvent(new ImportFinancialGuaranteeStatusChangeEvent(this, transition.Source, transition.Destination));
        }

        private void OnComplete(DateTime completedDate)
        {
            CompletedDate = completedDate;
        }

        public void UpdateReceivedDate(DateTime date)
        {
            ReceivedDate = date;
        }

        public void Complete(DateTime date)
        {
            if (!CompletedDate.HasValue)
            {
                stateMachine.Fire(completeTrigger, date);
            }
            else
            {
                CompletedDate = date;
            }
        }

        public void AddStatusChangeRecord(ImportFinancialGuaranteeStatusChange statusChange)
        {
            StatusChangeCollection.Add(statusChange);
        }

        private enum Trigger
        {
            Complete,
            Approve,
            Refuse,
            Release
        }
    }
}
