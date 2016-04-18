namespace EA.Iws.Requests.ImportNotificationAssessment.Transactions
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotificationAssessment)]
    public class AddNotificationPayment : IRequest<bool>
    {
        public Guid ImportNotificationId { get; private set; }

        public decimal Amount { get; private set; }

        public PaymentMethod PaymentMethod { get; private set; }

        public DateTime Date { get; private set; }

        public string ReceiptNumber { get; private set; }

        public string Comments { get; private set; }

        public AddNotificationPayment(Guid importNotificationId, 
            decimal amount, 
            PaymentMethod paymentMethod, 
            DateTime date, 
            string receiptNumber, 
            string comments)
        {
            Guard.ArgumentNotDefaultValue(() => importNotificationId, importNotificationId);

            if (paymentMethod == PaymentMethod.Cheque)
            {
                Guard.ArgumentNotNull(() => receiptNumber, receiptNumber);
            }

            ImportNotificationId = importNotificationId;
            Amount = amount;
            PaymentMethod = paymentMethod;
            Date = date;
            ReceiptNumber = receiptNumber;
            Comments = comments;
        }
    }
}
