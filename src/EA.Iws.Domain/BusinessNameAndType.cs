namespace EA.Iws.Domain
{
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class BusinessNameAndType
    {
        public string Name { get; private set; }

        public string Type { get; private set; }

        public string CompaniesHouseNumber { get; private set; }

        public string RegistrationNumber1 { get; private set; }

        public string RegistrationNumber2 { get; private set; }

        public BusinessNameAndType(string name, string type, string companiesHouseNumber, string registrationNumber1,
            string registrationNumber2)
        {
            Guard.ArgumentNotNull(name);
            Guard.ArgumentNotNull(type);

            Name = name;
            Type = type;
            CompaniesHouseNumber = companiesHouseNumber;
            RegistrationNumber1 = registrationNumber1;
            RegistrationNumber2 = registrationNumber2;
        }

        private BusinessNameAndType()
        {
        }
    }
}
