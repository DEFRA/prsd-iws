namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class TechnologyEmployed : Entity
    {
        private TechnologyEmployed(bool annexProvided, string details, string furtherDetails)
        {
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

            AnnexProvided = annexProvided;
            Details = details;
            FurtherDetails = furtherDetails;
        }

        protected TechnologyEmployed()
        {
        }

        public bool AnnexProvided { get; private set; }
        public string Details { get; private set; }
        public string FurtherDetails { get; private set; }

        public static TechnologyEmployed CreateTechnologyEmployedWithAnnex(string details)
        {
            return new TechnologyEmployed(true, details, null);
        }

        public static TechnologyEmployed CreateTechnologyEmployedWithFurtherDetails(string details, string furtherDetails)
        {
            Guard.ArgumentNotNullOrEmpty(() => details, details);
            return new TechnologyEmployed(false, details, furtherDetails);
        }
    }
}
