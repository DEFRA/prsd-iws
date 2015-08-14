namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using Prsd.Core.Mediator;

    public class SetOtherWasteCodes : IRequest<Guid>
    {
        public SetOtherWasteCodes(Guid notificationId, string exportNationalCode, string importNationalCode, string customsCode, string otherCode)
        {
            NotificationId = notificationId;
            ExportNationalCode = exportNationalCode;
            ImportNationalCode = importNationalCode;
            OtherCode = otherCode;
            CustomsCode = customsCode;
        }

        public Guid NotificationId { get; private set; }

        public string ExportNationalCode { get; private set; }

        public string ImportNationalCode { get; private set; }

        public string OtherCode { get; private set; }

        public string CustomsCode { get; private set; }
    }
}