namespace EA.Iws.Core.TechnologyEmployed
{
    using System;
    using System.Collections.Generic;
    using OperationCodes;

    public class TechnologyEmployedData
    {
        public Guid NotificationId { get; set; }
        public bool AnnexProvided { get; set; }
        public string Details { get; set; }
        public string FurtherDetails { get; set; }
        public bool HasTechnologyEmployed { get; set; }

        public IList<OperationCode> OperationCodes { get; set; }
    }
}