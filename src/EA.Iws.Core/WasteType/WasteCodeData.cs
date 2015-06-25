namespace EA.Iws.Core.WasteType
{
    using System;

    public class WasteCodeData
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public CodeType CodeType { get; set; }

        public Guid NotificationId { get; set; }

        public string OptionalCode { get; set; }

        public string OptionalDescription { get; set; }
    }
}