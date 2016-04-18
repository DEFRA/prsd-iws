namespace EA.Iws.Domain.TransportRoute
{
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class EntryOrExitPoint : Entity
    {
        public string Name { get; protected set; }

        public virtual Country Country { get; protected set; }
        
        protected EntryOrExitPoint()
        {
        }

        public EntryOrExitPoint(string name, Country country)
        {
            Guard.ArgumentNotNullOrEmpty(() => name, name);
            Guard.ArgumentNotNull(() => country, country);

            Name = name;
            Country = country;
        }
    }
}
