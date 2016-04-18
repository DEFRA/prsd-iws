namespace EA.Iws.Core.ImportNotification.Summary
{
    using System.Collections.Generic;
    using OperationCodes;

    public class WasteOperation
    {
        public IList<OperationCode> OperationCodes { get; set; }

        public string TechnologyEmployed { get; set; }
    }
}
