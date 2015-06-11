namespace EA.Iws.Domain
{
    using System;

    public class Country
    {
        protected Country()
        {
        }

        public Guid Id { get; protected set; }

        public string Name { get; private set; }

        public string IsoAlpha2Code { get; private set; }

        public bool IsEuropeanUnionMember { get; private set; }
    }
}