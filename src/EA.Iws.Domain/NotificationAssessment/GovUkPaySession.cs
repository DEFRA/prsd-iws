namespace EA.Iws.Domain.NotificationAssessment
{
    using EA.Prsd.Core.Domain;
    using Prsd.Core;
    using System;

    public class GovUkPaySession : Entity
    {
        public Guid NotificationId { get; private set; }

        public string PaymentId { get; private set; }

        public string PaymentReference { get; private set; }

        public string SecureToken { get; private set; }

        public decimal Amount { get; private set; }

        public string Status { get; private set; }

        public bool InFinalState { get; private set; }

        public Guid UserId { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public DateTime? UpdatedDate { get; private set; }

        protected GovUkPaySession()
        {
        }

        public GovUkPaySession(Guid notificationId,
            string paymentId,
            string paymentReference,
            string secureToken,
            decimal amount,
            Guid userId,
            DateTime createdDate)
        {
            Guard.ArgumentNotDefaultValue(() => notificationId, notificationId);
            Guard.ArgumentNotNullOrEmpty(() => paymentId, paymentId);
            Guard.ArgumentNotNullOrEmpty(() => paymentReference, paymentReference);
            Guard.ArgumentNotNullOrEmpty(() => secureToken, secureToken);
            Guard.ArgumentNotZeroOrNegative(() => amount, amount);
            Guard.ArgumentNotDefaultValue(() => userId, userId);

            NotificationId = notificationId;
            PaymentId = paymentId;
            PaymentReference = paymentReference;
            SecureToken = secureToken;
            Amount = amount;
            UserId = userId;
            Status = "created";
            InFinalState = false;
            CreatedDate = createdDate;
        }

        public void UpdateStatus(string status, bool inFinalState, DateTime updatedDate)
        {
            Status = status;
            InFinalState = inFinalState;
            UpdatedDate = updatedDate;
        }
    }
}