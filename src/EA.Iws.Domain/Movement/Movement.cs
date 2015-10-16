namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using Core.Movement;
    using Core.Shared;
    using NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using Stateless;
    using MovementReceiptDecision = Core.MovementReceipt.Decision;

    public class Movement : Entity
    {
        private readonly StateMachine<MovementStatus, Trigger> stateMachine;

        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<Guid> submittedTrigger;
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<DateTime, Guid> completedTrigger;
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<Guid, DateTime, string> rejectedTrigger;
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<Guid, DateTime, decimal> acceptedTrigger;

        private enum Trigger
        {
            Submit,
            Receive,
            Complete,
            Reject,
            Cancel
        }

        protected Movement()
        {
            stateMachine = CreateStateMachine();
        }

        internal Movement(int movementNumber, Guid notificationId)
        {
            Number = movementNumber;
            NotificationId = notificationId;
            Status = MovementStatus.New;
            StatusChangeCollection = new List<MovementStatusChange>();
            stateMachine = CreateStateMachine();
        }

        public int Number { get; private set; }

        public Guid NotificationId { get; private set; }

        public MovementStatus Status { get; private set; }

        protected virtual ICollection<MovementStatusChange> StatusChangeCollection { get; set; }

        public IEnumerable<MovementStatusChange> StatusChanges
        {
            get { return StatusChangeCollection.ToSafeIEnumerable(); }
        }

        public bool IsActive
        {
            get
            {
                return this.Date.HasValue
                    && this.Date < SystemTime.UtcNow;
            }
        }

        public bool IsReceived
        {
            get
            {
                return this.Receipt != null
                    && this.Receipt.Decision == MovementReceiptDecision.Accepted
                    && this.Receipt.Quantity.HasValue;
            }
        }

        public virtual MovementReceipt Receipt { get; private set; }

        public virtual MovementCompletedReceipt CompletedReceipt { get; private set; }

        public DateTime? Date { get; internal set; }

        public decimal? Quantity { get; private set; }

        public int? NumberOfPackages { get; private set; }

        public ShipmentQuantityUnits? Units { get; private set; }

        public Guid? FileId { get; private set; }

        protected virtual ICollection<PackagingInfo> PackagingInfosCollection { get; set; }

        protected virtual ICollection<MovementCarrier> MovementCarriersCollection { get; set; }

        public IEnumerable<PackagingInfo> PackagingInfos
        {
            get { return PackagingInfosCollection.ToSafeIEnumerable(); }
        }

        public IEnumerable<MovementCarrier> MovementCarriers
        {
            get { return MovementCarriersCollection.ToSafeIEnumerable(); }
        }

        public void SetQuantity(ShipmentQuantity shipmentQuantity)
        {
            Quantity = shipmentQuantity.Quantity;
            Units = shipmentQuantity.Units;
        }

        public void SetPackagingInfos(IEnumerable<PackagingInfo> packagingInfos)
        {
            PackagingInfosCollection.Clear();

            foreach (var packagingInfo in packagingInfos)
            {
                PackagingInfosCollection.Add(packagingInfo);
            }
        }

        public void SetNumberOfPackages(int number)
        {
            Guard.ArgumentNotZeroOrNegative(() => number, number);

            NumberOfPackages = number;
        }

        public void SetMovementCarriers(IEnumerable<MovementCarrier> movementCarriers)
        {
            MovementCarriersCollection.Clear();

            foreach (var movementCarrier in movementCarriers)
            {
                MovementCarriersCollection.Add(movementCarrier);
            }
        }
        
        public void AddStatusChangeRecord(MovementStatusChange statusChange)
        {
            Guard.ArgumentNotNull(() => statusChange, statusChange);

            StatusChangeCollection.Add(statusChange);
        }

        private StateMachine<MovementStatus, Trigger> CreateStateMachine()
        {
            var stateMachine = new StateMachine<MovementStatus, Trigger>(() => Status, s => Status = s);

            submittedTrigger = stateMachine.SetTriggerParameters<Guid>(Trigger.Submit);
            completedTrigger = stateMachine.SetTriggerParameters<DateTime, Guid>(Trigger.Complete);

            rejectedTrigger = stateMachine.SetTriggerParameters<Guid, DateTime, string>(Trigger.Reject);

            acceptedTrigger = stateMachine.SetTriggerParameters<Guid, DateTime, decimal>(Trigger.Receive);

            stateMachine.OnTransitioned(OnTransitionAction);

            stateMachine.Configure(MovementStatus.New)
                .Permit(Trigger.Submit, MovementStatus.Submitted);

            stateMachine.Configure(MovementStatus.Submitted)
                .OnEntryFrom(submittedTrigger, OnSubmitted)
                .Permit(Trigger.Receive, MovementStatus.Received)
                .Permit(Trigger.Reject, MovementStatus.Rejected)
                .Permit(Trigger.Cancel, MovementStatus.Cancelled);

            stateMachine.Configure(MovementStatus.Received)
                .OnEntryFrom(acceptedTrigger, OnReceived)
                .Permit(Trigger.Complete, MovementStatus.Completed);

            stateMachine.Configure(MovementStatus.Completed)
                .OnEntryFrom(completedTrigger, OnCompleted);
                
            stateMachine.Configure(MovementStatus.Rejected).OnEntryFrom(rejectedTrigger, OnRejected);
            
            return stateMachine;
        }

        private void OnTransitionAction(StateMachine<MovementStatus, Trigger>.Transition transition)
        {
            RaiseEvent(new MovementStatusChangeEvent(this, transition.Destination));
        }

        public void Submit(Guid fileId)
        {
            Guard.ArgumentNotDefaultValue(() => fileId, fileId);

            //TODO: Check the movement is in a valid state to submit

            stateMachine.Fire(submittedTrigger, fileId);
        }

        private void OnSubmitted(Guid fileId)
        {
            FileId = fileId;
        }

        public void Reject(Guid fileId, DateTime dateReceived, string reason)
        {
            Guard.ArgumentNotDefaultValue(() => fileId, fileId);
            Guard.ArgumentNotDefaultValue(() => dateReceived, dateReceived);
            Guard.ArgumentNotDefaultValue(() => reason, reason);

            stateMachine.Fire(rejectedTrigger, fileId, dateReceived, reason);
        }

        public void OnRejected(Guid fileId, DateTime dateReceived, string reason)
        {
            Receipt = new MovementReceipt(fileId, dateReceived, reason);
        }

        public void Receive(Guid fileId, DateTime dateReceived, decimal quantity)
        {
            Guard.ArgumentNotDefaultValue(() => fileId, fileId);
            Guard.ArgumentNotDefaultValue(() => dateReceived, dateReceived);
            Guard.ArgumentNotDefaultValue(() => quantity, quantity);

            stateMachine.Fire(acceptedTrigger, fileId, dateReceived, quantity);
        }

        public void OnReceived(Guid fileId, DateTime dateReceived, decimal quantity)
        {
            Receipt = new MovementReceipt(fileId, dateReceived, quantity);
        }

        public void Cancel()
        {
            stateMachine.Fire(Trigger.Cancel);
        }

        public void Complete(DateTime completedDate, Guid fileId)
        {
            Guard.ArgumentNotDefaultValue(() => completedDate, completedDate);
            Guard.ArgumentNotDefaultValue(() => fileId, fileId);

            stateMachine.Fire(completedTrigger, completedDate, fileId);
        }

        private void OnCompleted(DateTime completedDate, Guid fileId)
        {
            this.CompletedReceipt = new MovementCompletedReceipt(completedDate, fileId);
        }
    }
}
