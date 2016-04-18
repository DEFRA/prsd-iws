namespace EA.Iws.Domain
{
    using System;

    public class LocalArea
    {
        protected LocalArea()
        {
        }

        public Guid Id { get; protected set; }

        public string Name { get; protected set; }

        public int CompetentAuthorityId { get; protected set; }
    }
}
