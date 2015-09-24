namespace EA.Iws.DocumentGeneration.Movement.MovementBlocks
{
    using System.Collections.Generic;
    using System.Reflection;
    using Domain.NotificationApplication;
    using NotificationBlocks;
    using ViewModels;

    internal class MovementProducerBlock : ProducerBlock
    {
        private readonly PropertyInfo[] properties;

        public MovementProducerBlock(IList<MergeField> mergeFields, NotificationApplication notification) 
            : base(mergeFields, notification)
        {
            properties = PropertyHelper.GetPropertiesForViewModel(typeof(ProducerViewModel));
        }

        public override void Merge()
        {
            if (!HasAnnex)
            {
                if (Data.Count == 1)
                {
                    MergeProducerToMainDocument(Data[0].GetProducerViewModelShowingAnnexMessages(Data.Count, Data[0], 0), properties);
                }
            }
        }

        public override void GenerateAnnex(int annexNumber)
        {  
            //If there is only one producer but also an annex
            if (Data.Count == 1)
            {
                MergeProducerToMainDocument(Data[0].GetProducerViewModelShowingAnnexMessages(Data.Count, Data[0], annexNumber), properties);
            }

            //If there are two producers the one that is the site of generation goes in the annex
            //and the other goes on the notification document in block 9
            if (Data.Count > 1)
            {
                //If there is only one left put it on the form otherwise put them all in the annex
                MergeProducerToMainDocument(Data[0].GetProducerViewModelShowingAnnexMessages(Data.Count, Data[0], annexNumber), properties);
            }
        }
    }
}
