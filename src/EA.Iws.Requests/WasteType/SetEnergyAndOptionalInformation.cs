namespace EA.Iws.Requests.WasteType
{
    using System;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    public class SetEnergyAndOptionalInformation : IRequest<Guid>
    {
        public SetEnergyAndOptionalInformation(string energyInformation, string optionalInformation, bool hasAnnex, Guid notificationId)
        {
            EnergyInformation = energyInformation;
            NotificationId = notificationId;
            OptionalInformation = optionalInformation;
            HasAnnex = hasAnnex;
        }

        public string EnergyInformation { get; private set; }

        public string OptionalInformation { get; private set; }

        public bool HasAnnex { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}