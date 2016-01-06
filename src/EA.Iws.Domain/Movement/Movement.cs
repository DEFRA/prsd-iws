namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Extensions;
    using Stateless;

    public class Movement : Entity
    {
        private readonly StateMachine<MovementStatus, Trigger> stateMachine;

        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<Guid> submittedTrigger;
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<DateTime, Guid> completedTrigger;
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<Guid, DateTime, ShipmentQuantity> acceptedTrigger;
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<DateTime, ShipmentQuantity> internallyAcceptedTrigger;
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<DateTime> internallySubmittedTrigger;
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<DateTime> internallyCompletedTrigger;

        private enum Trigger
        {
            Submit,
            SubmitInternal,
            Receive,
            Complete,
            Reject,
            Cancel,
            ReceiveInternal,
            CompleteInternal
        }

        protected Movement()
        {
            stateMachine = CreateStateMachine();
            StatusChangeCollection = new List<MovementStatusChange>();
        }

        public static Movement Capture(int movementNumber, Guid notificationId, DateTime actualDate,
            DateTime? preNotificationDate)
        {
            var movement = new Movement
            {
                NotificationId = notificationId,
                Number = movementNumber,
                Date = actualDate,
                Status = MovementStatus.Captured
            };

            if (preNotificationDate.HasValue)
            {
                movement.SubmitInternally(preNotificationDate.Value);
            }

            return movement;
        }

        internal Movement(int movementNumber, Guid notificationId, DateTime date)
        {
            Number = movementNumber;
            NotificationId = notificationId;
            Date = date;

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
                return (Status == MovementStatus.Submitted
                    || Status == MovementStatus.Received)
                    && Date < SystemTime.UtcNow;
            }
        }

        public bool HasShipped
        {
            get
            {
                return Status == MovementStatus.Received && Date < SystemTime.UtcNow;
            }
        }

        public virtual MovementReceipt Receipt { get; private set; }

        public virtual MovementCompletedReceipt CompletedReceipt { get; private set; }

        public DateTime Date { get; internal set; }

        public Guid? FileId { get; private set; }

        public DateTimeOffset? PrenotificationDate { get; private set; }

        public void AddStatusChangeRecord(MovementStatusChange statusChange)
        {
            Guard.ArgumentNotNull(() => statusChange, statusChange);

            StatusChangeCollection.Add(statusChange);
        }

        public async Task UpdateDate(DateTime newDate, IUpdatedMovementDateValidator validator)
        {
            await validator.EnsureDateValid(this, newDate);

            var previousDate = Date;
            Date = newDate;

            RaiseEvent(new MovementDateChangeEvent(Id, previousDate));
        }

        private StateMachine<MovementStatus, Trigger> CreateStateMachine()
        {
            var stateMachine = new StateMachine<MovementStatus, Trigger>(() => Status, s => Status = s);

            submittedTrigger = stateMachine.SetTriggerParameters<Guid>(Trigger.Submit);
            internallySubmittedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.SubmitInternal);
            completedTrigger = stateMachine.SetTriggerParameters<DateTime, Guid>(Trigger.Complete);
            internallyCompletedTrigger = stateMachine.SetTriggerParameters<DateTime>(Trigger.CompleteInternal);

            acceptedTrigger = stateMachine.SetTriggerParameters<Guid, DateTime, ShipmentQuantity>(Trigger.Receive);
            internallyAcceptedTrigger = stateMachine.SetTriggerParameters<DateTime, ShipmentQuantity>(Trigger.ReceiveInternal);
            
            stateMachine.OnTransitioned(OnTransitionAction);

            stateMachine.Configure(MovementStatus.New)
                .Permit(Trigger.Submit, MovementStatus.Submitted);

            stateMachine.Configure(MovementStatus.Submitted)
                .OnEntryFrom(submittedTrigger, OnSubmitted)
                .OnEntryFrom(internallySubmittedTrigger, OnInternallySubmitted)
                .Permit(Trigger.Receive, MovementStatus.Received)
                .Permit(Trigger.ReceiveInternal, MovementStatus.Received)
                .Permit(Trigger.Reject, MovementStatus.Rejected)
                .Permit(Trigger.Cancel, MovementStatus.Cancelled);

            stateMachine.Configure(MovementStatus.Received)
                .OnEntryFrom(acceptedTrigger, OnReceived)
                .OnEntryFrom(internallyAcceptedTrigger, OnInternallyReceived)
                .Permit(Trigger.Complete, MovementStatus.Completed)
                .Permit(Trigger.CompleteInternal, MovementStatus.Completed);

            stateMachine.Configure(MovementStatus.Completed)
                .OnEntryFrom(completedTrigger, OnCompleted)
                .OnEntryFrom(internallyCompletedTrigger, OnInternallyCompleted);
            
            stateMachine.Configure(MovementStatus.Captured)
                .Permit(Trigger.ReceiveInternal, MovementStatus.Received)
                .Permit(Trigger.Reject, MovementStatus.Rejected)
                .Permit(Trigger.SubmitInternal, MovementStatus.Submitted);

            return stateMachine;
        }

        private void OnTransitionAction(StateMachine<MovementStatus, Trigger>.Transition transition)
        {
            RaiseEvent(new MovementStatusChangeEvent(this, transition.Destination));
        }

        public void Submit(Guid fileId)
        {
            Guard.ArgumentNotDefaultValue(() => fileId, fileId);

            stateMachine.Fire(submittedTrigger, fileId);
        }

        private void OnSubmitted(Guid fileId)
        {
            FileId = fileId;
            PrenotificationDate = SystemTime.UtcNow;
        }

        public void SubmitInternally(DateTime prenotificationDate)
        {
            stateMachine.Fire(internallySubmittedTrigger, prenotificationDate);
        }

        private void OnInternallySubmitted(DateTime prenotificationDate)
        {
            PrenotificationDate = new DateTimeOffset(prenotificationDate);
        }

        internal MovementRejection Reject(DateTimeOffset dateReceived, 
            string reason,
            string furtherDetails)
        {
            Guard.ArgumentNotDefaultValue(() => dateReceived, dateReceived);
            Guard.ArgumentNotDefaultValue(() => reason, reason);

            var rejection = new MovementRejection(Id, dateReceived, reason, furtherDetails);

            stateMachine.Fire(Trigger.Reject);

            return rejection;
        }

        public void Receive(Guid fileId, DateTime dateReceived, ShipmentQuantity quantity)
        {
            Guard.ArgumentNotDefaultValue(() => fileId, fileId);
            Guard.ArgumentNotDefaultValue(() => dateReceived, dateReceived);
            Guard.ArgumentNotDefaultValue(() => quantity, quantity);

            stateMachine.Fire(acceptedTrigger, fileId, dateReceived, quantity);
        }

        private void OnReceived(Guid fileId, DateTime dateReceived, ShipmentQuantity quantity)
        {
            Receipt = new MovementReceipt(fileId, dateReceived, quantity);
        }

        public void ReceiveInternally(DateTime dateReceived, ShipmentQuantity quantity)
        {
            Guard.ArgumentNotDefaultValue(() => dateReceived, dateReceived);
            Guard.ArgumentNotNull(() => quantity, quantity);

            stateMachine.Fire(internallyAcceptedTrigger, dateReceived, quantity);
        }

        private void OnInternallyReceived(DateTime dateReceived, ShipmentQuantity quantity)
        {
            Receipt = new MovementReceipt(dateReceived, quantity);
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
            CompletedReceipt = new MovementCompletedReceipt(completedDate, fileId);
        }

        public void CompleteInternally(DateTime completedDate)
        {
            stateMachine.Fire(internallyCompletedTrigger, completedDate);
        }

        private void OnInternallyCompleted(DateTime completedDate)
        {
            CompletedReceipt = new MovementCompletedReceipt(completedDate);
        }
    }
}