namespace EA.Iws.Core.Admin.Reports
{
    using System;

    public class UserActionData
    {
        public string OriginalValue { get; set; }

        public string NewValue { get; set; }

        public Guid RecordId { get; set; } 
    }
}
