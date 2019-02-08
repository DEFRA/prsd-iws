namespace EA.Iws.RequestHandlers.NotificationMovements.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.Movement.BulkReceiptRecovery;
    using Core.Shared;
    using Prsd.Core.Mapper;

    public class ReceiptRecoveryMovementCollectionMap : IMap<DataTable, List<ReceiptRecoveryMovement>>
    {
        private readonly IMap<ReceiptRecoveryDataRow, string> notificationNumberMapper;
        private readonly IMap<ReceiptRecoveryDataRow, int?> shipmentNumberMapper;
        private readonly IMap<ReceiptRecoveryDataRow, DateTime?> receivedDateMapper;
        private readonly IMap<ReceiptRecoveryDataRow, decimal?> quantityMapper;
        private readonly IMap<ReceiptRecoveryDataRow, ShipmentQuantityUnits?> unitsMapper;
        private readonly IMap<ReceiptRecoveryDataRow, DateTime?> recoveredDisposedDateMapper;

        public ReceiptRecoveryMovementCollectionMap(IMap<ReceiptRecoveryDataRow, string> notificationNumberMapper,
            IMap<ReceiptRecoveryDataRow, int?> shipmentNumberMapper,
            IMap<ReceiptRecoveryDataRow, DateTime?> receivedDateMapper,
            IMap<ReceiptRecoveryDataRow, decimal?> quantityMapper,
            IMap<ReceiptRecoveryDataRow, ShipmentQuantityUnits?> unitsMapper,
            IMap<ReceiptRecoveryDataRow, DateTime?> recoveredDisposedDateMapper)
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
            {
                var dataRow = new ReceiptRecoveryDataRow()
                {
                    DataRow = data,
                    IsReceivedDate = false
                };
                var dataRowReceivedData = new ReceiptRecoveryDataRow()
                {
                    DataRow = data,
                    IsReceivedDate = true
                };
                return new ReceiptRecoveryMovement()
                {
                    NotificationNumber = notificationNumberMapper.Map(dataRow),
                    ShipmentNumber = shipmentNumberMapper.Map(dataRow),
                    ReceivedDate = receivedDateMapper.Map(dataRowReceivedData),
                    Quantity = quantityMapper.Map(dataRow),
                    Unit = unitsMapper.Map(dataRow),
                    RecoveredDisposedDate = recoveredDisposedDateMapper.Map(dataRow),
                    MissingNotificationNumber = string.IsNullOrWhiteSpace(data.ItemArray[(int)ReceiptRecoveryColumnIndex.NotificationNumber].ToString()),
                    MissingShipmentNumber = string.IsNullOrWhiteSpace(data.ItemArray[(int)ReceiptRecoveryColumnIndex.ShipmentNumber].ToString()),
                    MissingReceivedDate = string.IsNullOrWhiteSpace(data.ItemArray[(int)ReceiptRecoveryColumnIndex.Received].ToString()),
                    MissingQuantity = string.IsNullOrWhiteSpace(data.ItemArray[(int)ReceiptRecoveryColumnIndex.Quantity].ToString()),
                    MissingUnits = string.IsNullOrWhiteSpace(data.ItemArray[(int)ReceiptRecoveryColumnIndex.Unit].ToString()),
                    MissingRecoveredDisposedDate = string.IsNullOrWhiteSpace(data.ItemArray[(int)ReceiptRecoveryColumnIndex.RecoveredDisposed].ToString())
                };
            }).ToList();
        }
    }
}
