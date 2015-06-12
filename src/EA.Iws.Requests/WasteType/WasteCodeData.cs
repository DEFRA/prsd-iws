namespace EA.Iws.Requests.WasteType
{
    using System;

    public class WasteCodeData
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public bool IsOecdCode { get; set; }

        public Guid NotificationId { get; set; }
    }
}