namespace EA.Iws.RequestHandlers.ImportNotification.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotification.Update;
    using Core.WasteCodes;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Core = Core.ImportNotification.Summary;
    using Domain = Domain.ImportNotification;

    internal class WasteTypeMap : IMap<Domain.WasteType, Core.WasteType>,
        IMapWithParameter<Domain.WasteType, List<WasteCodeData>, WasteTypes>
    {
        private readonly IWasteCodeRepository wasteCodeRepository;

        public WasteTypeMap(IWasteCodeRepository wasteCodeRepository)
        {
            this.wasteCodeRepository = wasteCodeRepository;
        }

        public Core.WasteType Map(Domain.WasteType source)
        {
            var wasteType = new Core.WasteType();

            if (source == null)
            {
                return wasteType;
            }

            wasteType.Name = source.Name;

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

            if (source.WasteCategoryType.HasValue)
            {
                wasteType.WasteCategoryType = source.WasteCategoryType.Value;
            }

            return wasteType;
        }

        public WasteTypes Map(Domain.WasteType source, List<WasteCodeData> wasteCodeData)
        {
            if (source == null)
            {
                return new WasteTypes();
            }

            var result = new WasteTypes(source.ImportNotificationId)
            {
                Name = source.Name,
                BaselCodeNotListed = source.BaselOecdCodeNotListed,
                HCodeNotApplicable = source.HCodeNotApplicable,
                YCodeNotApplicable = source.YCodeNotApplicable,
                UnClassNotApplicable = source.UnClassNotApplicable,
                AllCodes = wasteCodeData
            };

            var wasteCodes = Task.Run(() => wasteCodeRepository
                .GetWasteCodesByIds(source.WasteCodes.Select(x => x.WasteCodeId))).Result.ToList();

            if (!source.BaselOecdCodeNotListed)
            {
                var baselOecdCode = wasteCodes
                    .Single(x =>
                        x.CodeType == CodeType.Basel
                        || x.CodeType == CodeType.Oecd);

                var selectedBaselCode = source.WasteCodes.SingleOrDefault(wc => wc.WasteCodeId == baselOecdCode.Id);

                result.SelectedBaselCode = selectedBaselCode != null ? selectedBaselCode.WasteCodeId : (Guid?)null;
            }

            result.SelectedEwcCodes = wasteCodes.Where(x => x.CodeType == CodeType.Ewc).Select(code => code.Id).ToList();

            if (!source.HCodeNotApplicable)
            {
                result.SelectedHCodes = wasteCodes.Where(x => x.CodeType == CodeType.H).Select(code => code.Id).ToList();
            }

            if (!source.YCodeNotApplicable)
            {
                result.SelectedYCodes = wasteCodes.Where(x => x.CodeType == CodeType.Y).Select(code => code.Id).ToList();
            }

            if (!source.UnClassNotApplicable)
            {
                result.SelectedUnClasses = wasteCodes.Where(x => x.CodeType == CodeType.Un).Select(code => code.Id).ToList();
            }

            return result;
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

        private IList<Guid> MapWasteCodesToIds(CodeType codeType, IEnumerable<WasteCode> wasteCodes)
        {
            return wasteCodes.Where(x => x.CodeType == codeType).Select(code => code.Id).ToList();
        }
    }
}