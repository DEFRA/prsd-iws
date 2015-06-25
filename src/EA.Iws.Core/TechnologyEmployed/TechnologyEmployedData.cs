namespace EA.Iws.Core.TechnologyEmployed
{
    using System;

    public class TechnologyEmployedData
    {
        public Guid NotificationId { get; set; }
        public bool AnnexProvided { get; set; }
        public string Details { get; set; }
        public bool HasTechnologyEmployed { get; set; }
    }
}