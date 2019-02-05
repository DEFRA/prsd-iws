namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Shared;
    using Prsd.Core.Mapper;

    public class ReceiptRecoveryMovementCollectionMap : IMap<DataTable, List<ReceiptRecoveryMovement>>
    {
        private readonly IMap<DataRow, string> notificationNumberMapper;
        private readonly IMap<DataRow, int?> shipmentNumberMapper;
        private readonly IMap<DataRow, DateTime?> receivedDateMapper;
        private readonly IMap<DataRow, decimal?> quantityMapper;
        private readonly IMap<DataRow, ShipmentQuantityUnits?> unitsMapper;
        private readonly IMap<DataRow, DateTime?> recoveredDisposedDateMapper;

        public ReceiptRecoveryMovementCollectionMap(IMap<DataRow, string> notificationNumberMapper,
            IMap<DataRow, int?> shipmentNumberMapper,
            IMap<DataRow, DateTime?> receivedDateMapper,
            IMap<DataRow, decimal?> quantityMapper,
            IMap<DataRow, ShipmentQuantityUnits?> unitsMapper,
            IMap<DataRow, DateTime?> recoveredDisposedDateMapper)
        {
            this.notificationNumberMapper = notificationNumberMapper;
            this.shipmentNumberMapper = shipmentNumberMapper;
            this.receivedDateMapper = receivedDateMapper;
            this.quantityMapper = quantityMapper;
            this.unitsMapper = unitsMapper;
            this.recoveredDisposedDateMapper = recoveredDisposedDateMapper;
        }

        public List<ReceiptRecoveryMovement> Map(DataTable source)
        {
            return source.AsEnumerable().Select(data =>
                new ReceiptRecoveryMovement()
                {
                    NotificationNumber = notificationNumberMapper.Map(data),
                    ShipmentNumber = shipmentNumberMapper.Map(data),
                    ReceivedDate = receivedDateMapper.Map(data),
                    Quantity = quantityMapper.Map(data),
                    Unit = unitsMapper.Map(data),
                    RecoveredDisposedDate = recoveredDisposedDateMapper.Map(data),
                    MissingNotificationNumber = string.IsNullOrWhiteSpace(data.ItemArray[(int)ReceiptRecoveryColumnIndex.NotificationNumber].ToString()),
                    MissingShipmentNumber = string.IsNullOrWhiteSpace(data.ItemArray[(int)ReceiptRecoveryColumnIndex.ShipmentNumber].ToString()),
                    MissingReceivedDate = string.IsNullOrWhiteSpace(data.ItemArray[(int)ReceiptRecoveryColumnIndex.Received].ToString()),
                    MissingQuantity = string.IsNullOrWhiteSpace(data.ItemArray[(int)ReceiptRecoveryColumnIndex.Quantity].ToString()),
                    MissingUnits = string.IsNullOrWhiteSpace(data.ItemArray[(int)ReceiptRecoveryColumnIndex.Unit].ToString()),
                    MissingRecoveredDisposedDate = string.IsNullOrWhiteSpace(data.ItemArray[(int)ReceiptRecoveryColumnIndex.RecoveredDisposed].ToString())
                }).ToList();
        }
    }
}
