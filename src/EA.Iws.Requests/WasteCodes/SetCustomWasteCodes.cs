namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetCustomWasteCodes : IRequest<Guid>
    {
        public SetCustomWasteCodes(Guid notificationId,
            string exportNationalCode,
            bool exportNationalCodeNotApplicable,
            string importNationalCode,
            bool importNationalCodeNotApplicable,
            string customsCode,
            bool customsCodeNotApplicable,
            string otherCode,
            bool otherCodeNotApplicable)
        {
            NotificationId = notificationId;
            ExportNationalCode = exportNationalCode;
            ImportNationalCode = importNationalCode;
            OtherCode = otherCode;
            CustomsCode = customsCode;
            ExportNationalCodeNotApplicable = exportNationalCodeNotApplicable;
            ImportNationalCodeNotApplicable = importNationalCodeNotApplicable;
            OtherCodeNotApplicable = otherCodeNotApplicable;
            CustomsCodeNotApplicable = customsCodeNotApplicable;
        }

        public Guid NotificationId { get; private set; }

        public string ExportNationalCode { get; private set; }

        public bool ExportNationalCodeNotApplicable { get; private set; }

        public bool ImportNationalCodeNotApplicable { get; private set; }

        public bool OtherCodeNotApplicable { get; private set; }

        public bool CustomsCodeNotApplicable { get; private set; }

        public string ImportNationalCode { get; private set; }

        public string OtherCode { get; private set; }

        public string CustomsCode { get; private set; }
    }
}