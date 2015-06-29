namespace EA.Iws.Requests.WasteType
{
    using System;
    using Prsd.Core.Mediator;

    public class SetEnergyAndOptionalInformation : IRequest<Guid>
    {
        public SetEnergyAndOptionalInformation(string energyInformation, string optionalInformation, Guid notificationId)
        {
            EnergyInformation = energyInformation;
            NotificationId = notificationId;
            OptionalInformation = optionalInformation;
        }

        public string EnergyInformation { get; private set; }

        public string OptionalInformation { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}