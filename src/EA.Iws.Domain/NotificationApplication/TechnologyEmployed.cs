namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class TechnologyEmployed : Entity
    {
        private TechnologyEmployed(Guid notificationId, bool annexProvided, string details, string furtherDetails)
        {
            Guard.ArgumentNotDefaultValue(() => notificationId, notificationId);

            if (annexProvided && !string.IsNullOrEmpty(furtherDetails))
            {
                throw new InvalidOperationException(string.Format("NotificationId {0} - If AnnexProvided is selected then Further Details must not contain any text", Id));
            }

            if (string.IsNullOrEmpty(details))
            {
                throw new InvalidOperationException(string.Format("NotificationId {0} - Details must contain some text", Id));
            }

            if (details.Length > 70)
            {
                throw new InvalidOperationException(string.Format("NotificationId {0} - Details must not be more than 70 characters", Id));
            }

            NotificationId = notificationId;
            AnnexProvided = annexProvided;
            Details = details;
            FurtherDetails = furtherDetails;
        }

        protected TechnologyEmployed()
        {
        }

        public Guid NotificationId { get; private set; }

        public bool AnnexProvided { get; private set; }
        public string Details { get; private set; }
        public string FurtherDetails { get; private set; }

        public void SetWithAnnex(string details)
        {
            Guard.ArgumentNotNullOrEmpty(() => details, details);
            AnnexProvided = true;
            Details = details;
            FurtherDetails = null;
        }

        public void SetWithFurtherDetails(string details, string furtherDetails)
        {
            Guard.ArgumentNotNullOrEmpty(() => details, details);
            AnnexProvided = false;
            Details = details;
            FurtherDetails = furtherDetails;
        }

        public static TechnologyEmployed CreateTechnologyEmployedWithAnnex(Guid notificationId, string details)
        {
            return new TechnologyEmployed(notificationId, true, details, null);
        }

        public static TechnologyEmployed CreateTechnologyEmployedWithFurtherDetails(Guid notificationId, string details, string furtherDetails)
        {
            Guard.ArgumentNotNullOrEmpty(() => details, details);
            return new TechnologyEmployed(notificationId, false, details, furtherDetails);
        }
    }
}
