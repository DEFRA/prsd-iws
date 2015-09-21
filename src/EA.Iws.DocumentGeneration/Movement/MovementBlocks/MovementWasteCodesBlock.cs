namespace EA.Iws.DocumentGeneration.Movement.MovementBlocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using NotificationBlocks;

    internal class MovementWasteCodesBlock : WasteCodesBlock
    {
        public MovementWasteCodesBlock(IList<MergeField> mergeFields, NotificationApplication notification) 
            : base(mergeFields, notification)
        {
        }

        public override void GenerateAnnex(int annexNumber)
        {
            MergeToMainDocument(annexNumber);
        }
    }
}
