namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class WasteTypeMap : IMap<Domain.WasteType, Core.WasteType>
    {
        private readonly IWasteCodeRepository wasteCodeRepository;

        public WasteTypeMap(IWasteCodeRepository wasteCodeRepository)
        {
            this.wasteCodeRepository = wasteCodeRepository;
        }

        public Core.WasteType Map(Domain.WasteType source)
        {
            var wasteType = new Core.WasteType { Name = source.Name };

            var wasteCodes = Task.Run(() => wasteCodeRepository
                .GetWasteCodesByIds(source.WasteCodes.Select(x => x.WasteCodeId))).Result;

            if (source.BaselOecdCodeNotListed)
            {
                wasteType.BaselCode = new Core.WasteCodeSelection { IsNotApplicable = true };
            }
            else
            {
                var baselOecdCode = wasteCodes
                    .Single(x =>
                        x.CodeType == CodeType.Basel
                        || x.CodeType == CodeType.Oecd);

                var coreWasteCodes = new[]
                {
                    new Core.WasteCode
                    {
                        Name = baselOecdCode.Code,
                        Description = baselOecdCode.Description
                    }
                };

                wasteType.BaselCode = new Core.WasteCodeSelection
                {
                    IsNotApplicable = false,
                    WasteCodes = coreWasteCodes
                };
            }

            wasteType.EwcCodes = MapWasteCodes(CodeType.Ewc, wasteCodes);

            if (source.YCodeNotApplicable)
            {
                wasteType.YCodes = new Core.WasteCodeSelection { IsNotApplicable = true };
            }
            else
            {
                wasteType.YCodes = MapWasteCodes(CodeType.Y, wasteCodes);
            }

            if (source.HCodeNotApplicable)
            {
                wasteType.HCodes = new Core.WasteCodeSelection { IsNotApplicable = true };
            }
            else
            {
                wasteType.HCodes = MapWasteCodes(CodeType.H, wasteCodes);
            }

            if (source.UnClassNotApplicable)
            {
                wasteType.UnClasses = new Core.WasteCodeSelection { IsNotApplicable = true };
            }
            else
            {
                wasteType.UnClasses = MapWasteCodes(CodeType.Un, wasteCodes);
            }

            return wasteType;
        }

        private Core.WasteCodeSelection MapWasteCodes(CodeType codeType, IEnumerable<WasteCode> wasteCodes)
        {
            var coreWasteCodes = new List<Core.WasteCode>();

            foreach (var code in wasteCodes.Where(x => x.CodeType == codeType))
            {
                var coreWasteCode = new Core.WasteCode
                {
                    Name = code.Code,
                    Description = code.Description
                };

                coreWasteCodes.Add(coreWasteCode);
            }

            return new Core.WasteCodeSelection
            {
                IsNotApplicable = false,
                WasteCodes = coreWasteCodes
            };
        }
    }
}