namespace EA.Iws.Core.ImportNotificationAssessment
{
    using System.Collections.Generic;

    public class ImportNotificationCommentsUsersData
    {
        public IDictionary<string, string> Users { get; set; }

        public ImportNotificationCommentsUsersData()
        {
            this.Users = new Dictionary<string, string>();
        }
    }
}
