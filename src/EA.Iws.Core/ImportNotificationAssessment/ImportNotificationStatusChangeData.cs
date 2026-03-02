using EA.Iws.Core.Registration.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EA.Iws.Core.ImportNotificationAssessment
{
    public class ImportNotificationStatusChangeData
    {
        public ImportNotificationStatus Status { get; set; }

        public string UserId { get; set; }

        public string FullName { get; set; }

        public User User { get; set; }

        public DateTime ChangeDate { get; set; }

    }
}
