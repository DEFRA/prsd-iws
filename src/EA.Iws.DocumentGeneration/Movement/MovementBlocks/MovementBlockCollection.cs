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
                new ExporterBlock(mergeFields, notification),
                new FacilityBlock(mergeFields, notification),
                new ImporterBlock(mergeFields, notification),
                new ProducerBlock(mergeFields, notification),
                new WasteCodesBlock(mergeFields, notification),
                new WasteCompositionBlock(mergeFields, notification),
                new OperationBlock(mergeFields, notification),
                new MovementCarrierBlock(mergeFields, movement)
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
