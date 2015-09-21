namespace EA.Iws.DocumentGeneration.Movement.MovementBlocks
{
    using System.Collections;
    using System.Collections.Generic;
    using Domain.Movement;
    using NotificationBlocks;

    internal class MovementBlockCollection : IEnumerable<IDocumentBlock>
    {
        private readonly List<IDocumentBlock> movementDocumentBlocks;

        public MovementBlockCollection(IList<MergeField> mergeFields, Movement movement)
        {
            var notification = movement.NotificationApplication;

            movementDocumentBlocks = new List<IDocumentBlock>
            {
                new MovementBlock(mergeFields, movement),
                new MovementFacilityBlock(mergeFields, notification),
                new MovementOperationBlock(mergeFields, notification),
                new ImporterBlock(mergeFields, notification),
                new ExporterBlock(mergeFields, notification),
                new MovementProducerBlock(mergeFields, notification),
                new MovementCarrierBlock(mergeFields, movement),
                new MovementWasteCodesBlock(mergeFields, notification),
                new MovementWasteCompositionBlock(mergeFields, notification),
                new MovementSpecialHandlingBlock(mergeFields, notification)
            };
        }

        public IEnumerator<IDocumentBlock> GetEnumerator()
        {
            return movementDocumentBlocks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return movementDocumentBlocks.GetEnumerator();
        }
    }
}
