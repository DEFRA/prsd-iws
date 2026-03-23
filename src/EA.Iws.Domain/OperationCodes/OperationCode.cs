namespace EA.Iws.Domain.OperationCodes
{
    using EA.Iws.Core.Shared;

    public class OperationCode
    {
        public OperationCode()
        {
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsInterim { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}
