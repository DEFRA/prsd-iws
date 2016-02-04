namespace EA.Iws.Domain.ImportNotificationAssessment.FinancialGuarantee
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Admin;
    using Core.ImportNotificationAssessment.FinancialGuarantee;
    using Core.Shared;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using Stateless;

    public class ImportFinancialGuarantee : Entity
    {
        private static readonly BidirectionalDictionary<Trigger, FinancialGuaranteeDecision> DecisionDictionary = new BidirectionalDictionary<Trigger, FinancialGuaranteeDecision>(
            new Dictionary<Trigger, FinancialGuaranteeDecision>
            {
                { Trigger.Approve, FinancialGuaranteeDecision.Approved },
                { Trigger.Refuse, FinancialGuaranteeDecision.Refused },
                { Trigger.Release, FinancialGuaranteeDecision.Released }
            }); 

        private readonly StateMachine<ImportFinancialGuaranteeStatus, Trigger> stateMachine;

        private StateMachine<ImportFinancialGuaranteeStatus, Trigger>.TriggerWithParameters<DateTime> completeTrigger;  

        public Guid ImportNotificationId { get; private set; }

        public ImportFinancialGuaranteeStatus Status { get; private set; }

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
            Status = ImportFinancialGuaranteeStatus.ApplicationReceived;
        }

        private StateMachine<ImportFinancialGuaranteeStatus, Trigger> CreateStateMachine()
        {
            var stateMachine = new StateMachine<ImportFinancialGuaranteeStatus, Trigger>(() => Status, s => Status = s);

            stateMachine.OnTransitioned(OnTransitionAction);

            completeTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.Complete);

            stateMachine.Configure(ImportFinancialGuaranteeStatus.ApplicationReceived)
                .Permit(Trigger.Complete, ImportFinancialGuaranteeStatus.ApplicationComplete);

            stateMachine.Configure(ImportFinancialGuaranteeStatus.ApplicationComplete)
                .OnEntryFrom(completeTrigger, OnComplete)
                .Permit(Trigger.Approve, ImportFinancialGuaranteeStatus.Approved)
                .Permit(Trigger.Refuse, ImportFinancialGuaranteeStatus.Refused);

            stateMachine.Configure(ImportFinancialGuaranteeStatus.Approved)
                .Permit(Trigger.Release, ImportFinancialGuaranteeStatus.Released);
                
            return stateMachine;
        }

        private void OnTransitionAction(StateMachine<ImportFinancialGuaranteeStatus, Trigger>.Transition transition)
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

        public IEnumerable<FinancialGuaranteeDecision> GetAvailableDecisions()
        {
            return stateMachine.PermittedTriggers
                .Where(t => DecisionDictionary.ContainsKey(t))
                .Select(t => DecisionDictionary[t]);
        }

        private enum Trigger
        {
            Complete,
            Approve,
            Refuse,
            Release
        }

        internal ImportFinancialGuaranteeApproval Approve(DateTime date, DateRange validDates, int activeLoads, string reference)
        {
            stateMachine.Fire(Trigger.Approve);

            return ImportFinancialGuaranteeApproval.CreateApproval(ImportNotificationId, date, validDates, activeLoads, reference);
        }

        internal ImportFinancialGuaranteeApproval ApproveBlanketBond(DateTime date, DateTime validFrom, int activeLoads, string bondReference)
        {
            stateMachine.Fire(Trigger.Approve);

            return ImportFinancialGuaranteeApproval.CreateBlanketBondApproval(ImportNotificationId, date, validFrom, activeLoads, bondReference);
        }

        public ImportFinancialGuaranteeRelease Release(DateTime date)
        {
            stateMachine.Fire(Trigger.Release);

            return new ImportFinancialGuaranteeRelease(ImportNotificationId, date);
        }

        public ImportFinancialGuaranteeRefusal Refuse(DateTime date, string reason)
        {
            stateMachine.Fire(Trigger.Refuse);

            return new ImportFinancialGuaranteeRefusal(ImportNotificationId, date, reason);
        }
    }
}
