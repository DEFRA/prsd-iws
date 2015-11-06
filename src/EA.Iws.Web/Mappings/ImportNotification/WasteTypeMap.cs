namespace EA.Iws.Web.Mappings.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Areas.ImportNotification.ViewModels.Shared;
    using Areas.ImportNotification.ViewModels.WasteType;
    using Core.ImportNotification.Draft;
    using Core.WasteCodes;
    using Newtonsoft.Json;
    using Prsd.Core.Mapper;

    public class WasteTypeMap : IMap<WasteTypeViewModel, WasteType>,
        IMapWithParameter<WasteType, List<WasteCodeData>, WasteTypeViewModel>
    {
        private readonly IMapper mapper;

        public WasteTypeMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public WasteType Map(WasteTypeViewModel source)
        {
            var wasteType = new WasteType
            {
                Name = source.Name,
                BaselCodeNotListed = source.BaselCodeNotListed,
                YCodeNotApplicable = source.YCodeNotApplicable,
                HCodeNotApplicable = source.HCodeNotApplicable,
                UnClassNotApplicable = source.UnClassNotApplicable,
            };

            if (!source.BaselCodeNotListed)
            {
                wasteType.SelectedBaselCode = source.SelectedBaselCode;
            }

            if (source.SelectedEwcCodesJson != null)
            {
                wasteType.SelectedEwcCodes = JsonConvert.DeserializeObject<List<Guid>>(source.SelectedEwcCodesJson);
            }

            if (source.SelectedYCodesJson != null && !source.YCodeNotApplicable)
            {
                wasteType.SelectedYCodes = JsonConvert.DeserializeObject<List<Guid>>(source.SelectedYCodesJson);
            }

            if (source.SelectedHCodesJson != null && !source.HCodeNotApplicable)
            {
                wasteType.SelectedHCodes = JsonConvert.DeserializeObject<List<Guid>>(source.SelectedHCodesJson);                
            }

            if (source.SelectedUnClassesJson != null && !source.UnClassNotApplicable)
            {
                wasteType.SelectedUnClasses = JsonConvert.DeserializeObject<List<Guid>>(source.SelectedUnClassesJson);
            }
            
            return wasteType;
        }

        public WasteTypeViewModel Map(WasteType source, List<WasteCodeData> parameter)
        {
            var model = new WasteTypeViewModel(source);

            if (source.SelectedEwcCodes != null)
            {
                model.SelectedEwcCodesJson = JsonConvert.SerializeObject(source.SelectedEwcCodes);
                model.SelectedEwcCodesDisplay = mapper.Map<List<WasteCodeViewModel>>(parameter
                    .Where(p =>
                        source.SelectedEwcCodes.Contains(p.Id))
                    .ToList());
            }

            if (source.SelectedYCodes != null)
            {
                model.SelectedYCodesJson = JsonConvert.SerializeObject(source.SelectedYCodes);
                model.SelectedYCodesDisplay = mapper.Map<List<WasteCodeViewModel>>(parameter
                    .Where(p =>
                        source.SelectedYCodes.Contains(p.Id))
                    .ToList());
            }

            if (source.SelectedHCodes != null)
            {
                model.SelectedHCodesJson = JsonConvert.SerializeObject(source.SelectedHCodes);
                model.SelectedHCodesDisplay = mapper.Map<List<WasteCodeViewModel>>(parameter
                    .Where(p =>
                        source.SelectedHCodes.Contains(p.Id))
                    .ToList());
            }

            if (source.SelectedUnClasses != null)
            {
                model.SelectedUnClassesJson = JsonConvert.SerializeObject(source.SelectedUnClasses);
                model.SelectedUnClassesDisplay = mapper.Map<List<WasteCodeViewModel>>(parameter
                    .Where(p =>
                        source.SelectedUnClasses.Contains(p.Id))
                        .ToList());
            }

            model.AllCodes = mapper.Map<List<WasteCodeViewModel>>(parameter);

            return model;
        }
    }
}