﻿namespace EA.Iws.Core.Admin.Search
{
    using System;
    using ImportNotificationAssessment;
    using Shared;

    public class ImportSearchResult
    {
        public Guid Id { get; set; }

        public string NotificationNumber { get; set; }

        public NotificationType NotificationType { get; set; }

        public ImportNotificationStatus Status { get; set; }

        public bool ShowShipmentSummaryLink { get; set; }

        public string Importer { get; set; }
    }
}
