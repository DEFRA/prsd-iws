﻿namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using System.ComponentModel;

    [DisplayName("Transport route import")]
    public class StateOfImport : IDraftEntity
    {
        public Guid? CompetentAuthorityId { get; set; }

        public Guid? EntryPointId { get; set; }

        public Guid ImportNotificationId { get; private set; }

        public StateOfImport(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        internal StateOfImport()
        {
        }
    }
}
