namespace EA.Iws.RequestHandlers.Mappings.Movement
{
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core.Mapper;

    public class MovementFileMap : IMap<Movement, MovementFileData>
    {
        public MovementFileData Map(Movement source)
        {
            var result = new MovementFileData
            {
                ShipmentNumber = source.Number,
                PrenotificationFileId = source.FileId
            };

            if (source.Receipt != null)
            {
                result.ReceiptFileId = source.Receipt.FileId;
            }

            if (source.CompletedReceipt != null)
            {
                result.OperationReceiptFileId = source.CompletedReceipt.FileId;
            }

            return result;
        }
    }
}