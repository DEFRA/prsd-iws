namespace EA.Iws.DocumentGeneration.Movement.Blocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using Mapper;
    using Notification.Blocks;
    using ViewModels;

    internal class MovementOperationBlock : OperationBlock
    {
        public MovementOperationBlock(IList<MergeField> mergeFields, NotificationApplication notification, TechnologyEmployed technologyEmployed) 
            : base(mergeFields, notification, technologyEmployed)
        {
        }

        public override void Merge()
        {
            if (!HasAnnex)
            {
                var properties = PropertyHelper.GetPropertiesForViewModel(typeof(OperationViewModel));
                foreach (var field in CorrespondingMergeFields)
                {
                    MergeFieldDataMapper.BindCorrespondingField(field, Data, properties);
                }
            }
        }

        public override void GenerateAnnex(int annexNumber)
        {
            MergeOperationToMainDocument(annexNumber);
        }
    }
}
