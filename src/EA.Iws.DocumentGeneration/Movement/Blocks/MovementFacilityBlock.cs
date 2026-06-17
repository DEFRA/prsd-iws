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

        public MovementFacilityBlock(IList<MergeField> mergeFields, FacilityCollection facilityCollection)
            : base(mergeFields, facilityCollection)
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
            // Always display the first facility (by ordinal position) in Block 10
            // with full name and address, per regulatory requirements.
            MergeFacilityToMainDocument(FacilityViewModel
                .GetFirstFacilityWithAnnexReference(Data[0], annexNumber), properties);
        }
    }
}