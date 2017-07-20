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
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<DateTime, Guid, Guid> completedTrigger;
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<AcceptedTriggerParameters> acceptedTrigger;
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<InternallyAcceptedTriggerParameters> internallyAcceptedTrigger;
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<DateTime> internallySubmittedTrigger;
        private StateMachine<MovementStatus, Trigger>.TriggerWithParameters<DateTime, Guid> internallyCompletedTrigger;

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
            DateTime? prenotificationDate, bool hasNoPrenotification, Guid createdBy)
        {
            if (hasNoPrenotification && prenotificationDate.HasValue)
            {
                throw new ArgumentException("Can't provide prenotification date if there is no prenotification", "prenotificationDate");
            }

            var movement = new Movement
            {
                NotificationId = notificationId,
                Number = movementNumber,
                Date = actualDate,
                Status = MovementStatus.Captured,
                HasNoPrenotification = hasNoPrenotification,
                CreatedBy = createdBy.ToString()
            };

            if (prenotificationDate.HasValue)
            {
                movement.SubmitInternally(prenotificationDate.Value);
            }

            return movement;
        }

        internal Movement(int movementNumber, Guid notificationId, DateTime date, Guid createdBy)
        {
            Number = movementNumber;
            NotificationId = notificationId;
            Date = date;
            CreatedBy = createdBy.ToString();

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

        public virtual MovementReceipt Receipt { get; private set; }

        public virtual MovementCompletedReceipt CompletedReceipt { get; private set; }

        public DateTime Date { get; internal set; }

        public Guid? FileId { get; private set; }

        public DateTime? PrenotificationDate { get; private set; }

        public bool? HasNoPrenotification { get; set; }

        public bool HasShipped
        {
            get { return Status == MovementStatus.Submitted && Date < SystemTime.UtcNow; }
        }

        public string CreatedBy { get; private set; }

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
            completedTrigger = stateMachine.SetTriggerParameters<DateTime, Guid, Guid>(Trigger.Complete);
            internallyCompletedTrigger = stateMachine.SetTriggerParameters<DateTime, Guid>(Trigger.CompleteInternal);

            acceptedTrigger = stateMachine.SetTriggerParameters<AcceptedTriggerParameters>(Trigger.Receive);
            internallyAcceptedTrigger = stateMachine.SetTriggerParameters<InternallyAcceptedTriggerParameters>(Trigger.ReceiveInternal);

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
            PrenotificationDate = prenotificationDate;
        }

        internal MovementRejection Reject(DateTime dateReceived,
            string reason,
            string furtherDetails)
        {
            Guard.ArgumentNotDefaultValue(() => dateReceived, dateReceived);
            Guard.ArgumentNotDefaultValue(() => reason, reason);

            var rejection = new MovementRejection(Id, dateReceived, reason, furtherDetails);

            stateMachine.Fire(Trigger.Reject);

            return rejection;
        }

        public void Receive(Guid fileId, DateTime dateReceived, ShipmentQuantity quantity, Guid createdBy)
        {
            Guard.ArgumentNotDefaultValue(() => fileId, fileId);
            Guard.ArgumentNotDefaultValue(() => dateReceived, dateReceived);
            Guard.ArgumentNotDefaultValue(() => quantity, quantity);

            stateMachine.Fire(acceptedTrigger, new AcceptedTriggerParameters(fileId, dateReceived, quantity, createdBy));
        }

        private void OnReceived(AcceptedTriggerParameters parameters)
        {
            Receipt = new MovementReceipt(parameters.FileId, parameters.DateReceived, parameters.Quantity, parameters.CreatedBy);
        }

        public void ReceiveInternally(DateTime dateReceived, ShipmentQuantity quantity, Guid createdBy)
        {
            Guard.ArgumentNotDefaultValue(() => dateReceived, dateReceived);
            Guard.ArgumentNotNull(() => quantity, quantity);

            if (dateReceived < Date)
            {
                throw new InvalidOperationException("The when the waste was received date cannot be before the actual date of shipment.");
            }
            if (dateReceived > SystemTime.UtcNow.Date)
            {
                throw new InvalidOperationException("The when the waste was received date cannot be in the future.");
            }

            stateMachine.Fire(internallyAcceptedTrigger, new InternallyAcceptedTriggerParameters(dateReceived, quantity, createdBy));
        }

        private void OnInternallyReceived(InternallyAcceptedTriggerParameters parameters)
        {
            Receipt = new MovementReceipt(parameters.DateReceived, parameters.Quantity, parameters.CreatedBy);
        }

        public void Cancel()
        {
            stateMachine.Fire(Trigger.Cancel);
        }

        public void Complete(DateTime completedDate, Guid fileId, Guid createdBy)
        {
            Guard.ArgumentNotDefaultValue(() => completedDate, completedDate);
            Guard.ArgumentNotDefaultValue(() => fileId, fileId);

            stateMachine.Fire(completedTrigger, completedDate, fileId, createdBy);
        }

        private void OnCompleted(DateTime completedDate, Guid fileId, Guid createdBy)
        {
            CompletedReceipt = new MovementCompletedReceipt(completedDate, fileId, createdBy);
        }

        public void CompleteInternally(DateTime completedDate, Guid createdBy)
        {
            if (completedDate < Receipt.Date)
            {
                throw new InvalidOperationException("The when was the waste recovered date cannot be before the when was the waste received. ");
            }
            if (completedDate > SystemTime.UtcNow.Date)
            {
                throw new InvalidOperationException("The when the waste was recovered date cannot be in the future.");
            }
            stateMachine.Fire(internallyCompletedTrigger, completedDate, createdBy);
        }

        private void OnInternallyCompleted(DateTime completedDate, Guid createdBy)
        {
            CompletedReceipt = new MovementCompletedReceipt(completedDate, createdBy);
        }

        private class AcceptedTriggerParameters
        {
            public Guid FileId { get; private set; }

            public DateTime DateReceived { get; private set; }

            public ShipmentQuantity Quantity { get; private set; }

            public Guid CreatedBy { get; private set; }

            public AcceptedTriggerParameters(Guid fileId, DateTime dateReceived, ShipmentQuantity quantity, Guid createdBy)
            {
                FileId = fileId;
                DateReceived = dateReceived;
                CreatedBy = createdBy;
                Quantity = quantity;
            }
        }

        private class InternallyAcceptedTriggerParameters
        {
            public DateTime DateReceived { get; private set; }

            public ShipmentQuantity Quantity { get; private set; }

            public Guid CreatedBy { get; private set; }

            public InternallyAcceptedTriggerParameters(DateTime dateReceived, ShipmentQuantity quantity, Guid createdBy)
            {
                DateReceived = dateReceived;
                CreatedBy = createdBy;
                Quantity = quantity;
            }
        }
    }
}