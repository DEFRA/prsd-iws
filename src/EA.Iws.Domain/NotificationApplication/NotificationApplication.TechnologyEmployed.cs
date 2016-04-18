namespace EA.Iws.Domain.NotificationApplication
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