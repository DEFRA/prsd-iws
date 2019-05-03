namespace EA.Iws.Web.Mappings.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Areas.ImportNotification.ViewModels.Shared;
    using Areas.ImportNotification.ViewModels.UpdateJourney;
    using Areas.ImportNotification.ViewModels.WasteCodes;
    using Core.ImportNotification.Draft;
    using Core.ImportNotification.Update;
    using Core.WasteCodes;
    using Newtonsoft.Json;
    using Prsd.Core.Mapper;

    public class WasteCodesMap : IMap<WasteCodesViewModel, WasteType>,
        IMapWithParameter<WasteType, List<WasteCodeData>, WasteCodesViewModel>,
        IMap<WasteTypes, UpdateWasteCodesViewModel>,
        IMap<UpdateWasteCodesViewModel, WasteTypes>,
        IMapWithParameter<UpdateWasteCodesViewModel, List<WasteCodeData>, UpdateWasteCodesViewModel>
    {
        private readonly IMapper mapper;

        public WasteCodesMap(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public WasteType Map(WasteCodesViewModel source)
        {
            var wasteType = new WasteType(source.ImportNotificationId)
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

        public WasteCodesViewModel Map(WasteType source, List<WasteCodeData> parameter)
        {
            var model = new WasteCodesViewModel(source);
            model.ImportNotificationId = source.ImportNotificationId;

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

        public UpdateWasteCodesViewModel Map(WasteTypes source)
        {
            var model = new UpdateWasteCodesViewModel(source) { ImportNotificationId = source.ImportNotificationId };

            if (source.SelectedEwcCodes != null)
            {
                model.SelectedEwcCodesJson = JsonConvert.SerializeObject(source.SelectedEwcCodes);
                model.SelectedEwcCodesDisplay = mapper.Map<List<WasteCodeViewModel>>(source.AllCodes
                    .Where(p =>
                        source.SelectedEwcCodes.Contains(p.Id))
                    .ToList());
            }

            if (source.SelectedYCodes != null)
            {
                model.SelectedYCodesJson = JsonConvert.SerializeObject(source.SelectedYCodes);
                model.SelectedYCodesDisplay = mapper.Map<List<WasteCodeViewModel>>(source.AllCodes
                    .Where(p =>
                        source.SelectedYCodes.Contains(p.Id))
                    .ToList());
            }

            if (source.SelectedHCodes != null)
            {
                model.SelectedHCodesJson = JsonConvert.SerializeObject(source.SelectedHCodes);
                model.SelectedHCodesDisplay = mapper.Map<List<WasteCodeViewModel>>(source.AllCodes
                    .Where(p =>
                        source.SelectedHCodes.Contains(p.Id))
                    .ToList());
            }

            if (source.SelectedUnClasses != null)
            {
                model.SelectedUnClassesJson = JsonConvert.SerializeObject(source.SelectedUnClasses);
                model.SelectedUnClassesDisplay = mapper.Map<List<WasteCodeViewModel>>(source.AllCodes
                    .Where(p =>
                        source.SelectedUnClasses.Contains(p.Id))
                        .ToList());
            }

            model.AllCodes = mapper.Map<List<WasteCodeViewModel>>(source.AllCodes);

            return model;
        }

        public UpdateWasteCodesViewModel Map(UpdateWasteCodesViewModel source, List<WasteCodeData> parameter)
        {
            if (source.SelectedEwcCodesJson != null)
            {
                var selectedCodes = JsonConvert.DeserializeObject<List<Guid>>(source.SelectedEwcCodesJson);
                source.SelectedEwcCodesDisplay = mapper.Map<List<WasteCodeViewModel>>(parameter
                    .Where(p =>
                        selectedCodes.Contains(p.Id))
                    .ToList());
            }

            if (source.SelectedYCodesJson != null)
            {
                var selectedCodes = JsonConvert.DeserializeObject<List<Guid>>(source.SelectedYCodesJson);
                source.SelectedYCodesDisplay = mapper.Map<List<WasteCodeViewModel>>(parameter
                    .Where(p =>
                        selectedCodes.Contains(p.Id))
                    .ToList());
            }

            if (source.SelectedHCodesJson != null)
            {
                var selectedCodes = JsonConvert.DeserializeObject<List<Guid>>(source.SelectedHCodesJson);
                source.SelectedHCodesDisplay = mapper.Map<List<WasteCodeViewModel>>(parameter
                    .Where(p =>
                        selectedCodes.Contains(p.Id))
                    .ToList());
            }

            if (source.SelectedUnClassesJson != null)
            {
                var selectedCodes = JsonConvert.DeserializeObject<List<Guid>>(source.SelectedUnClassesJson);
                source.SelectedUnClassesDisplay = mapper.Map<List<WasteCodeViewModel>>(parameter
                    .Where(p =>
                        selectedCodes.Contains(p.Id))
                        .ToList());
            }

            source.AllCodes = mapper.Map<List<WasteCodeViewModel>>(parameter);

            return source;
        }

        public WasteTypes Map(UpdateWasteCodesViewModel source)
        {
            var wasteType = new WasteTypes(source.ImportNotificationId)
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
    }
}