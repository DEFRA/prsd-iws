namespace EA.Iws.RequestHandlers.ImportNotification.Summary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using Core.ImportNotification.Summary;
    using DataAccess.Draft;
    using Draft = Core.ImportNotification.Draft;

    [AutoRegister]
    public class WasteTypeSummary
    {
        private readonly IDraftImportNotificationRepository draftRepository;
        private readonly Domain.NotificationApplication.IWasteCodeRepository wasteCodeRepository;

        public WasteTypeSummary(IDraftImportNotificationRepository draftRepository, 
            Domain.NotificationApplication.IWasteCodeRepository wasteCodeRepository)
        {
            this.draftRepository = draftRepository;
            this.wasteCodeRepository = wasteCodeRepository;
        }

        public async Task<WasteType> GetWasteType(Guid notificationId)
        {
            var wasteType = await draftRepository.GetDraftData<Draft.WasteType>(notificationId);

            var lookups = await GetWasteCodesLookup(wasteType);

            return new WasteType
            {
                Name = wasteType.Name,
                EwcCodes = GetWasteCodeSelection(wasteType.SelectedEwcCodes, false, lookups),
                HCodes = GetWasteCodeSelection(wasteType.SelectedHCodes, wasteType.HCodeNotApplicable, lookups),
                UnClasses =
                    GetWasteCodeSelection(wasteType.SelectedUnClasses, wasteType.UnClassNotApplicable, lookups),
                YCodes = GetWasteCodeSelection(wasteType.SelectedYCodes, wasteType.YCodeNotApplicable, lookups),
                BaselCode = GetWasteCodeSelection(wasteType.SelectedBaselCode.HasValue ? 
                    new List<Guid>
                    {
                        wasteType.SelectedBaselCode.Value
                    }
                    : new List<Guid>(), wasteType.BaselCodeNotListed, lookups)
            };
        }

        public async Task<IEnumerable<Domain.NotificationApplication.WasteCode>> GetWasteCodesLookup(Draft.WasteType wasteType)
        {
            var wasteCodeIds = new List<Guid>();

            wasteCodeIds.AddRange(GetWasteCodeIdsToLookup(wasteType.SelectedEwcCodes));
            wasteCodeIds.AddRange(GetWasteCodeIdsToLookup(wasteType.SelectedHCodes));
            wasteCodeIds.AddRange(GetWasteCodeIdsToLookup(wasteType.SelectedUnClasses));
            wasteCodeIds.AddRange(GetWasteCodeIdsToLookup(wasteType.SelectedYCodes));

            if (wasteType.SelectedBaselCode.HasValue)
            {
                wasteCodeIds.Add(wasteType.SelectedBaselCode.Value);
            }

            return await wasteCodeRepository.GetWasteCodesByIds(wasteCodeIds);
        }

        private IEnumerable<Guid> GetWasteCodeIdsToLookup(List<Guid> wasteCodes)
        {
            if (wasteCodes != null && wasteCodes.Count > 0)
            {
                return wasteCodes;
            }

            return new Guid[0];
        }

        private WasteCodeSelection GetWasteCodeSelection(List<Guid> wasteCodes,
            bool notApplicable,
            IEnumerable<Domain.NotificationApplication.WasteCode> codeLookups)
        {
            if (notApplicable)
            {
                return new WasteCodeSelection
                {
                    IsNotApplicable = true
                };
            }

            if (wasteCodes == null)
            {
                return new WasteCodeSelection
                {
                    WasteCodes = new List<WasteCode>()
                };
            }

            return new WasteCodeSelection
            {
                WasteCodes = codeLookups.Where(c => wasteCodes.Contains(c.Id)).Select(wc => new WasteCode
                {
                    Name = wc.Code,
                    Description = wc.Description
                }).ToArray()
            };
        }
    }
}
