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
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<Guid, DateTime, string> rejectedTrigger;
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<Guid, DateTime, ShipmentQuantity> acceptedTrigger;

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
            StatusChangeCollection = new List<MovementStatusChange>();
        }

        public static Movement Capture(int movementNumber, Guid notificationId, DateTime actualDate,
            DateTime? preNotificationDate)
        {
            return new Movement
            {
                NotificationId = notificationId,
                Number = movementNumber,
                Date = actualDate,
                Status = MovementStatus.Captured
            };
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

        public void AddStatusChangeRecord(MovementStatusChange statusChange)
        {
            Guard.ArgumentNotNull(() => statusChange, statusChange);

            StatusChangeCollection.Add(statusChange);
        }

        public async Task UpdateDate(DateTime newDate, GetOriginalDate originalDateService)
        {
            if (Status != MovementStatus.Submitted)
            {
                throw new InvalidOperationException(string.Format("Cannot edit movement date when status is {0}", Status));
            }

            var originalDate = await originalDateService.Get(this);

            if (newDate > originalDate.AddDays(10))
            {
                throw new InvalidOperationException(string.Format(
                    "Cannot set movement date to {0} because it is more than 10 days after the original date of {1}.", 
                    newDate, 
                    originalDate));
            }

            var previousDate = Date;
            Date = newDate;

            RaiseEvent(new MovementDateChangeEvent(Id, previousDate));
        }

        private StateMachine<MovementStatus, Trigger> CreateStateMachine()
        {
            var stateMachine = new StateMachine<MovementStatus, Trigger>(() => Status, s => Status = s);

            submittedTrigger = stateMachine.SetTriggerParameters<Guid>(Trigger.Submit);
            completedTrigger = stateMachine.SetTriggerParameters<DateTime, Guid>(Trigger.Complete);

            rejectedTrigger = stateMachine.SetTriggerParameters<Guid, DateTime, string>(Trigger.Reject);

            acceptedTrigger = stateMachine.SetTriggerParameters<Guid, DateTime, ShipmentQuantity>(Trigger.Receive);

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

        public void Receive(Guid fileId, DateTime dateReceived, ShipmentQuantity quantity)
        {
            Guard.ArgumentNotDefaultValue(() => fileId, fileId);
            Guard.ArgumentNotDefaultValue(() => dateReceived, dateReceived);
            Guard.ArgumentNotDefaultValue(() => quantity, quantity);

            stateMachine.Fire(acceptedTrigger, fileId, dateReceived, quantity);
        }

        public void OnReceived(Guid fileId, DateTime dateReceived, ShipmentQuantity quantity)
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