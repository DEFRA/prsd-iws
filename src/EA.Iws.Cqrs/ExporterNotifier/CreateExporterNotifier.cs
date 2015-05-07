namespace EA.Iws.Cqrs.ExporterNotifier
{
    using System;
    using Core.Cqrs;

    public class CreateExporter : ICommand
    {
        public Guid NotificationId { get; private set; }

        public string RegistrationNumber { get; private set; }

        public string Name { get; private set; }

        public string CountryName { get; private set; }

        public string Building { get; private set; }

        public CreateExporter(Guid notificationId, 
            string registrationNumber, 
            string name,
            string countryName,
            string buildin)
        {
            NotificationId = notificationId;
            RegistrationNumber = registrationNumber;
            Name = name;
        }
    }
}
