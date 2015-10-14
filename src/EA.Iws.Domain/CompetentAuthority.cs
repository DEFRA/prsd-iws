namespace EA.Iws.Domain
{
    using System;

    public class CompetentAuthority
    {
        public Guid Id { get; protected set; }

        public virtual Country Country { get; protected set; }

        public string Name { get; protected set; }

        public string Abbreviation { get; protected set; }

        public string Code { get; protected set; }

        public bool IsSystemUser { get; protected set; }

        public bool? IsTransitAuthority { get; protected set; }

        protected CompetentAuthority()
        {
        }
    }
}
