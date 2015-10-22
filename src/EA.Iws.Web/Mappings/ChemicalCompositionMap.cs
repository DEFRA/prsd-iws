namespace EA.Iws.Web.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.ChemicalComposition;
    using Core.WasteType;
    using Prsd.Core.Mapper;

    public class ChemicalCompositionMap
        : IMapWithParameter<WasteTypeData, ICollection<WoodInformationData>, ChemicalCompositionViewModel>
    {
        private const string NotApplicable = "NA";

        public ChemicalCompositionViewModel Map(WasteTypeData source, 
            ICollection<WoodInformationData> parameter)
        {
            var result = new ChemicalCompositionViewModel();

            if (source.WasteAdditionalInformation.Any())
            {
                result.WasteComposition = SetExistingInformations(parameter, 
                    source);
            }
            else
            {
                result.WasteComposition = parameter.ToList();
            }

            result.Energy = source.EnergyInformation;

            return result;
        }

        private List<WoodInformationData> SetExistingInformations(ICollection<WoodInformationData> compositions, 
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