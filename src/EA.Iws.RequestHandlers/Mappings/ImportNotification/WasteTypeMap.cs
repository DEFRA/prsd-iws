namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using System;
    using Core.ImportNotification.Draft;
    using Domain.ImportNotification.WasteCodes;
    using Core.WasteCodes;
    using Prsd.Core.Mapper;

    internal class WasteTypeMap : IMap<WasteType, Domain.ImportNotification.WasteType>
    {
        public Domain.ImportNotification.WasteType Map(WasteType source)
        {
            BaselOecdCode baselCode;
            if (source.BaselCodeNotListed)
            {
                baselCode = BaselOecdCode.CreateNotListed();
            }
            else
            {
                baselCode = BaselOecdCode.CreateFor(GenerateWasteCode(source.SelectedBaselCode.Value, FUCK));
            }

            var ewcCode = EwcCode.CreateFor()

            return new Domain.ImportNotification.WasteType(source.ImportNotificationId,
                source.Name,
                baselCode,)
            throw new NotImplementedException();
        }

        private WasteCode GenerateWasteCode(Guid wasteCodeId, CodeType type)
        {
            return new WasteCode(wasteCodeId, type);
        }
    }
}