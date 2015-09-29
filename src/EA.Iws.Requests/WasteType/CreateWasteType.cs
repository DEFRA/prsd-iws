namespace EA.Iws.Requests.WasteType
{
    using System;
    using System.Collections.Generic;
    using Core.WasteType;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    public class CreateWasteType : IRequest<Guid>
    {
        public Guid NotificationId { get; set; }

        public ChemicalCompositionType ChemicalCompositionType { get; set; }
   
        public string ChemicalCompositionDescription { get; set; }

        public List<WasteTypeCompositionData> WasteCompositions { get; set; }

        public string WasteCompositionName { get; set; }
    }
}
