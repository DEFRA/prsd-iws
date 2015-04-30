namespace EA.Iws.Api.Client.Entities
{
    public class CreateNotificationData
    {
        public CreateNotificationData(CompetentAuthority competentAuthority, WasteAction wasteAction)
        {
            CompetentAuthority = competentAuthority;
            WasteAction = wasteAction;
        }

        public CompetentAuthority CompetentAuthority { get; private set; }

        public WasteAction WasteAction { get; private set; }
    }
}