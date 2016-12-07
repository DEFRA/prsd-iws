namespace EA.Iws.RequestHandlers.Mappings.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotification.Draft;
    using Domain.ImportNotification.WasteCodes;
    using Prsd.Core.Mapper;
    using ChemicalComposition = Core.WasteType.ChemicalComposition;

    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Variables relate to waste codes")]
    internal class WasteTypeMap : IMapWithParameter<WasteType, ChemicalComposition, Domain.ImportNotification.WasteType>
    {
        private readonly Domain.NotificationApplication.IWasteCodeRepository wasteCodeRepository;

        public WasteTypeMap(Domain.NotificationApplication.IWasteCodeRepository wasteCodeRepository)
        {
            this.wasteCodeRepository = wasteCodeRepository;
        }

        public Domain.ImportNotification.WasteType Map(WasteType source, ChemicalComposition parameter)
        {
            var baselOecdCode = CreateBaselCode(source.BaselCodeNotListed, source.SelectedBaselCode);
            var ewcCode = CreateEwcCode(source.SelectedEwcCodes);
            var yCode = CreateYCode(source.YCodeNotApplicable, source.SelectedYCodes);
            var hCode = CreateHCode(source.HCodeNotApplicable, source.SelectedHCodes);
            var unClass = CreateUnClass(source.UnClassNotApplicable, source.SelectedUnClasses);

            return new Domain.ImportNotification.WasteType(source.ImportNotificationId,
                source.Name,
                baselOecdCode,
                ewcCode,
                yCode,
                hCode,
                unClass,
                parameter);
        }

        private BaselOecdCode CreateBaselCode(bool notListed, Guid? codeId)
        {
            if (notListed)
            {
                return BaselOecdCode.CreateNotListed();
            }

            if (!codeId.HasValue)
            {
                throw new ArgumentNullException("codeId must not be null when basel/oecd code isn't 'not listed'");
            }

            var wasteCode = Task.Run(() => wasteCodeRepository.GetWasteCodesByIds(new[] { codeId.Value })).Result.Single();

            return BaselOecdCode.CreateFor(wasteCode);
        }

        private EwcCode CreateEwcCode(IEnumerable<Guid> codeIds)
        {
            var wasteCodes = Task.Run(() => wasteCodeRepository.GetWasteCodesByIds(codeIds)).Result;

            return EwcCode.CreateFor(wasteCodes);
        }

        private YCode CreateYCode(bool notApplicable, IEnumerable<Guid> codeIds)
        {
            if (notApplicable)
            {
                return YCode.CreateNotApplicable();
            }

            var wasteCodes = Task.Run(() => wasteCodeRepository.GetWasteCodesByIds(codeIds)).Result;

            return YCode.CreateFor(wasteCodes);
        }

        private HCode CreateHCode(bool notApplicable, IEnumerable<Guid> codeIds)
        {
            if (notApplicable)
            {
                return HCode.CreateNotApplicable();
            }

            var wasteCodes = Task.Run(() => wasteCodeRepository.GetWasteCodesByIds(codeIds)).Result;

            return HCode.CreateFor(wasteCodes);
        }

        private UnClass CreateUnClass(bool notApplicable, IEnumerable<Guid> codeIds)
        {
            if (notApplicable)
            {
                return UnClass.CreateNotApplicable();
            }

            var wasteCodes = Task.Run(() => wasteCodeRepository.GetWasteCodesByIds(codeIds)).Result;

            return UnClass.CreateFor(wasteCodes);
        }
    }
}