namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using Core.WasteCodes;

    public class WasteCode
    {
        protected WasteCode()
        {
        }

        public Guid Id { get; private set; }

        public string Description { get; private set; }

        public string Code { get; private set; }

        public CodeType CodeType { get; private set; }
    }
}