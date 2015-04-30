namespace EA.Iws.Domain.Notification
{
    using Core.Domain;

    public class WasteAction : Enumeration
    {
        public static readonly WasteAction Recovery = new WasteAction(1, "Recovery");
        public static readonly WasteAction Disposal = new WasteAction(2, "Disposal");

        private WasteAction()
        {
        }

        private WasteAction(int value, string displayName)
            : base(value, displayName)
        {
        }
    }
}