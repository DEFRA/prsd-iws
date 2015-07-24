namespace EA.Iws.Domain.Notification
{
    public partial class NotificationApplication
    {
        public bool HasTechnologyEmployed
        {
            get { return TechnologyEmployed != null; }
        }

        public void SetTechnologyEmployed(TechnologyEmployed technologyEmployed)
        {
            TechnologyEmployed = technologyEmployed;
        }
    }
}