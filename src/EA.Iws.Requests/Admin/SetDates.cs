namespace EA.Iws.Requests.Admin
{
    using System;
    using Prsd.Core.Mediator;

    public class SetDates : IRequest<Guid>
    {
        public Guid NotificationApplicationId { get; set; }

        public DateTime? NotificationReceivedDate { get; set; }

        public DateTime? PaymentRecievedDate { get; set; }

        public DateTime? CommencementDate { get; set; }

        public DateTime? CompleteDate { get; set; }

        public DateTime? TransmittedDate { get; set; }

        public DateTime? AcknowledgedDate { get; set; }

        public DateTime? DecisionDate { get; set; }

        public string NameOfOfficer { get; set; }
    }
}
