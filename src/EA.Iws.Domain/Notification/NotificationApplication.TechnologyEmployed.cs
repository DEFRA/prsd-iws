namespace EA.Iws.Domain.Notification
{
    using System;

    public partial class NotificationApplication
    {
        public bool HasTechnologyEmployed
        {
            get { return TechnologyEmployed != null; }
        }

        public void UpdateTechnologyEmployed(bool annexProvided, string details)
        {
            if (TechnologyEmployed == null)
            {
                TechnologyEmployed = new TechnologyEmployed(annexProvided, details);
            }
        }
    }
}
