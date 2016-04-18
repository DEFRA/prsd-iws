namespace EA.Iws.DocumentGeneration.Movement.Blocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using Notification.Blocks;
    using ViewModels;

    internal class MovementSpecialHandlingBlock : SpecialHandlingBlock
    {
        public MovementSpecialHandlingBlock(IList<MergeField> mergeFields, NotificationApplication notification) 
            : base(mergeFields, notification)
        {
        }

        public override void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(SpecialHandlingViewModel));
            MergeSpecialHandlingDataToDocument(Data, properties);
        }

        public override void GenerateAnnex(int annexNumber)
        {
        }
    }
}
