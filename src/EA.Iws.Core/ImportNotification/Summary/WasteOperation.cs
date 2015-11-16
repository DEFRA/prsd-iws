namespace EA.Iws.Core.ImportNotification.Summary
{
    using System.Collections.Generic;

    public class WasteOperation
    {
        public string DisposalOrRecoveryPrefix { get; set; }

        public IList<int> OperationCodes { get; set; }

        public string TechnologyEmployed { get; set; }
    }
}
