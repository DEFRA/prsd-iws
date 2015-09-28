namespace EA.Iws.Requests.WasteRecovery
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class SetWasteDisposal : IRequest<Guid>
    {
        public Guid NotificationId { get; private set; }
        public string Method { get; private set; }
        public decimal Amount { get; private set; }
        public ValuePerWeightUnits Unit { get; private set; }

        public SetWasteDisposal(Guid notificationId, string method, decimal amount, ValuePerWeightUnits unit)
        {
            NotificationId = notificationId;
            Method = method;
            Amount = amount;
            Unit = unit;
        }
    }
}
