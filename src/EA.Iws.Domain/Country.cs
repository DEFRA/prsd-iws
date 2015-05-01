namespace EA.Iws.Domain
{
    using Core.Domain;

    public class Country : Entity
    {
        public string Name { get; private set; }

        public string IsoAlpha2Code { get; private set; }

        public bool IsEuropeanUnionMember { get; private set; }

        private Country()
        {
        }
    }
}
