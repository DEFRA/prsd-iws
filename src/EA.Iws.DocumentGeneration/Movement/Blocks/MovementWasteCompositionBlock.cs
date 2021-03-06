﻿namespace EA.Iws.DocumentGeneration.Movement.Blocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using Mapper;
    using Notification.Blocks;
    using ViewModels;

    internal class MovementWasteCompositionBlock : WasteCompositionBlock
    {
        public MovementWasteCompositionBlock(IList<MergeField> mergeFields, NotificationApplication notification) 
            : base(mergeFields, notification)
        {
        }

        public override void Merge()
        {
            if (!HasAnnex)
            {
                var properties = PropertyHelper.GetPropertiesForViewModel(typeof(WasteCompositionViewModel));

                foreach (var field in CorrespondingMergeFields)
                {
                    MergeFieldDataMapper.BindCorrespondingField(field, Data, properties);
                }
            }
        }

        public override void GenerateAnnex(int annexNumber)
        {
            MergeToMainDocument(annexNumber);
        }
    }
}
