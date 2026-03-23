namespace EA.Iws.Core.OperationCodes
{
    using EA.Iws.Core.Shared;

    public class OperationCodeData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsInterim { get; set; }

        public NotificationType NotificiationType { get; set; }
    }
}
