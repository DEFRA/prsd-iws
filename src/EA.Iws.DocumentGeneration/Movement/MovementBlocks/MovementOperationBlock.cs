namespace EA.Iws.DocumentGeneration.Movement.MovementBlocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using NotificationBlocks;

    internal class MovementOperationBlock : OperationBlock
    {
        public MovementOperationBlock(IList<MergeField> mergeFields, NotificationApplication notification) 
            : base(mergeFields, notification)
        {
        }

        public override void GenerateAnnex(int annexNumber)
        {
            MergeOperationToMainDocument(annexNumber);
        }
    }
}
