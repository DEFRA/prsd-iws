﻿namespace EA.Iws.Requests.Notification
{
    using System.Collections.Generic;

    public class UserArchiveNotifications
    {
        public UserArchiveNotifications(IList<NotificationArchiveSummaryData> notifications, int numberOfNotifications, int pageNumber, int pageSize)
        {
            Notifications = notifications;
            NumberOfNotifications = numberOfNotifications;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int NumberOfNotifications { get; private set; }

        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }

        public IList<NotificationArchiveSummaryData> Notifications { get; private set; }
    }
}