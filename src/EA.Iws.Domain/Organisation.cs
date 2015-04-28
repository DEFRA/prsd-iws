namespace EA.Iws.Domain
{
    using Core.Domain;
    using Utils;

    public class Organisation : Entity, ICommand
    {
        public string Name { get; private set; }

        public Address Address { get; private set; }

        public string Type { get; private set; }

        public Organisation(string name, Address address, string type)
        {
            Guard.ArgumentNotNull(name);
            Guard.ArgumentNotNull(address);
            Guard.ArgumentNotNull(type);

            this.Name = name;
            this.Address = address;
            this.Type = type;
        }

        private Organisation()
        {
        }
    }
}
