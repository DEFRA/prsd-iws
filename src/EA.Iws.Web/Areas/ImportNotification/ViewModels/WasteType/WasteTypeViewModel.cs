namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteType
{
    using Core.WasteType;
    using Prsd.Core.Helpers;
    using Web.ViewModels.Shared;

    public class WasteTypeViewModel
    {
        public RadioButtonStringCollectionViewModel ChemicalCompositionType { get; set; }

        public WasteTypeViewModel()
        {
            ChemicalCompositionType = RadioButtonStringCollectionViewModel.CreateFromEnum<ChemicalComposition>();
        }

        public ChemicalComposition GetSelectedChemicalComposition()
        {
            if (ChemicalCompositionType.SelectedValue == EnumHelper.GetDisplayName(ChemicalComposition.Other))
            {
                return ChemicalComposition.Other;
            }

            if (ChemicalCompositionType.SelectedValue == EnumHelper.GetDisplayName(ChemicalComposition.RDF))
            {
                return ChemicalComposition.RDF;
            }

            if (ChemicalCompositionType.SelectedValue == EnumHelper.GetDisplayName(ChemicalComposition.SRF))
            {
                return ChemicalComposition.SRF;
            }

            if (ChemicalCompositionType.SelectedValue == EnumHelper.GetDisplayName(ChemicalComposition.Wood))
            {
                return ChemicalComposition.Wood;
            }

            return default(ChemicalComposition);
        }
    }
}