namespace EA.Iws.Core.NotificationAssessment
{
    using System.Collections.Generic;

    public class NotificationCommentsUsersData
    {
        public IDictionary<string, string> Users { get; set; }

        public NotificationCommentsUsersData()
        {
            this.Users = new Dictionary<string, string>();
        }
    }
}
