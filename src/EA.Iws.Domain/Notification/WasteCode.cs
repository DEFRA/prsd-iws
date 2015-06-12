namespace EA.Iws.Domain.Notification
{
    using Prsd.Core.Domain;

    public class WasteCode : Entity
    {
        protected WasteCode()
        {
        }

        public string Description { get; private set; }

        public string Code { get; private set; }

        public bool IsOecdCode { get; private set; }
    }
}