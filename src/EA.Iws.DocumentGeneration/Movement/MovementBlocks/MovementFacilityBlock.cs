namespace EA.Iws.DocumentGeneration.Movement.MovementBlocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using NotificationBlocks;
    using ViewModels;

    internal class MovementFacilityBlock : FacilityBlock
    {
        public MovementFacilityBlock(IList<MergeField> mergeFields, NotificationApplication notification)
            : base(mergeFields, notification)
        {
        }

        public override void GenerateAnnex(int annexNumber)
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(FacilityViewModel));

            if (Data.Count == 2)
            {
                var facility = (Data[0].IsActualSite) ? Data[1] : Data[0];
                MergeFacilityToMainDocument(FacilityViewModel
                    .GetSeeAnnexInstructionForFacilityCaseTwoFacilities(facility, annexNumber), properties);
            }
            else
            {
                //The main document should show "See Annex" for all data if there is an annex.
                MergeFacilityToMainDocument(FacilityViewModel.GetSeeAnnexInstructionForFacility(annexNumber), properties);
            }
        }
    }
}
