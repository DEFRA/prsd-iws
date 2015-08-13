namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Core.WasteCodes;

    public class WasteCode
    {
        protected WasteCode()
        {
        }

        public Guid Id { get; protected set; }

        public string Description { get; protected set; }

        public string Code { get; protected set; }

        public CodeType CodeType { get; protected set; }
    }
}