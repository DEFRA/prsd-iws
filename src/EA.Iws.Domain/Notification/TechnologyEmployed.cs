namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core.Domain;

    public class TechnologyEmployed : Entity
    {
        internal TechnologyEmployed(bool annexProvided, string details)
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

        public bool AnnexProvided { get; internal set; }
        public string Details { get; internal set; }
    }
}
