namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class TechnologyEmployed : Entity
    {
        private TechnologyEmployed(bool annexProvided, string details)
        {
            if (annexProvided && !string.IsNullOrEmpty(details))
            {
                throw new InvalidOperationException(string.Format("NotificationId {0} - If AnnexProvided is selected then Details must not contain any text", Id));
            }

            if (!annexProvided && string.IsNullOrEmpty(details))
            {
                throw new InvalidOperationException(string.Format("NotificationId {0} - If AnnexProvided is not selected then Details must contain some text", Id));
            }

            AnnexProvided = annexProvided;
            Details = details;
        }

        protected TechnologyEmployed()
        {
        }

        public bool AnnexProvided { get; private set; }
        public string Details { get; private set; }

        public static TechnologyEmployed CreateTechnologyEmployedInAnnex()
        {
            return new TechnologyEmployed(true, null);
        }

        public static TechnologyEmployed CreateTechnologyEmployedDetails(string details)
        {
            Guard.ArgumentNotNullOrEmpty(() => details, details);
            return new TechnologyEmployed(false, details);
        }
    }
}
