namespace EA.Iws.Requests.ImportNotification.WasteType
{
    using System.Collections.Generic;
    using Core.WasteCodes;
    using Prsd.Core.Mediator;

    public class GetAllWasteCodes : IRequest<IList<WasteCodeData>>
    {
    }
}