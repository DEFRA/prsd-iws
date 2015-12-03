namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System.Collections.Generic;
    using Core.ImportNotification.Draft;

    public interface IDraftImportNotificationValidator
    {
        IEnumerable<string> Validate(ImportNotification notificationDraft);
    }
}