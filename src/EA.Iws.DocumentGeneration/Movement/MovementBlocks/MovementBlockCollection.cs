namespace EA.Iws.DocumentGeneration.Movement.MovementBlocks
{
    using System.Collections;
    using System.Collections.Generic;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using NotificationBlocks;

    internal class MovementBlockCollection : IEnumerable<IDocumentBlock>
    {
        private readonly List<IDocumentBlock> movementDocumentBlocks;

        public MovementBlockCollection(IList<MergeField> mergeFields, 
            Movement movement, 
            NotificationApplication notification, 
            ShipmentInfo shipmentInfo)
        {
            movementDocumentBlocks = new List<IDocumentBlock>
            {
                new MovementBlock(mergeFields, movement, notification, shipmentInfo),
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
