namespace EA.Iws.DocumentGeneration.Movement.Blocks
{
    using System.Collections.Generic;
    using System.Reflection;
    using Domain.NotificationApplication;
    using Notification.Blocks;
    using ViewModels;

    internal class MovementFacilityBlock : FacilityBlock
    {
        private readonly PropertyInfo[] properties;

        public MovementFacilityBlock(IList<MergeField> mergeFields, NotificationApplication notification)
            : base(mergeFields, notification)
        {
            properties = PropertyHelper.GetPropertiesForViewModel(typeof(FacilityViewModel));
        }

        public override void Merge()
        {
            if (!HasAnnex)
            {
                if (Data.Count == 1)
                {
                    MergeFacilityToMainDocument(Data[0], properties);
                }
            }
        }

        public override void GenerateAnnex(int annexNumber)
        {
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
