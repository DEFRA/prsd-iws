namespace EA.Iws.RequestHandlers.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteType;
    using Domain.NotificationApplication;
    using Prsd.Core;
    using Prsd.Core.Mapper;

    internal class WasteAdditionalInformationMap : IMap<IList<WoodInformationData>, IList<WasteAdditionalInformation>>
    {
        public IList<WasteAdditionalInformation> Map(IList<WoodInformationData> source)
        {
            Guard.ArgumentNotNull(() => source, source);

            var result = source.Select(i =>
                WasteAdditionalInformation.CreateWasteAdditionalInformation(i.Constituent,
                    Convert.ToDecimal(i.MinConcentration),
                    Convert.ToDecimal(i.MaxConcentration),
                    i.WasteInformationType));

            return result.ToList();
        }
    }
}