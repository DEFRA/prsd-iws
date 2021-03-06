﻿namespace EA.Iws.Core.ImportNotification.Draft
{
    using System;
    using System.ComponentModel;

    [DisplayName("Pre-consent")]
    public class Preconsented : IDraftEntity
    {
        public Preconsented(Guid importNotificationId)
        {
            ImportNotificationId = importNotificationId;
        }

        internal Preconsented()
        {
        }

        public Guid ImportNotificationId { get; private set; }

        public bool? AllFacilitiesPreconsented { get; set; }
    }
}