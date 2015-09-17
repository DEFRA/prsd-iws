namespace EA.Iws.Web.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.WasteType;
    using Core.WasteType;
    using Prsd.Core.Mapper;

    public class ChemicalCompositionAdditionalInformationMap
        : IMapWithParameter<WasteTypeData, ICollection<WoodInformationData>, ChemicalCompositionInformationViewModel>
    {
        private const string NotApplicable = "NA";

        public ChemicalCompositionInformationViewModel Map(WasteTypeData source, 
            ICollection<WoodInformationData> parameter)
        {
            var result = new ChemicalCompositionInformationViewModel();

            if (source.WasteAdditionalInformation.Any())
            {
                result.WasteComposition = SetExistingAdditionalInformations(parameter, 
                    source);
            }
            else
            {
                result.WasteComposition = parameter.ToList();
            }

            result.Energy = source.EnergyInformation;
            result.FurtherInformation = source.FurtherInformation;
            result.HasAnnex = source.HasAnnex;

            return result;
        }

        private List<WoodInformationData> SetExistingAdditionalInformations(ICollection<WoodInformationData> compositions, 
            WasteTypeData existingData)
        {
            var result = new List<WoodInformationData>(existingData.WasteAdditionalInformation);

            var notApplicableCompositions =
                       compositions.Where(
                           wc =>
                               existingData.WasteAdditionalInformation
                               .All(c => c.WasteInformationType != wc.WasteInformationType))
                               .Select(wc => new WoodInformationData
                               {
                                   Constituent = wc.Constituent,
                                   WasteInformationType = wc.WasteInformationType,
                                   MinConcentration = NotApplicable,
                                   MaxConcentration = NotApplicable
                               });

            result.AddRange(notApplicableCompositions);

            return result;
        }
    }
}