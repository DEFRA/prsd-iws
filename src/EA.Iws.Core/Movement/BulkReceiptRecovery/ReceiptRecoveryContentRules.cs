﻿namespace EA.Iws.Core.Movement.BulkReceiptRecovery
{
    using System.ComponentModel.DataAnnotations;

    public enum ReceiptRecoveryContentRules
    {
        [Display(Name = "The maximum number of shipments that can be uploaded is {0}")]
        MaximumShipments,
        [Display(Name = "All rows of data must contain valid notification and/or shipment numbers.")]
        InvalidNotificationOrShipmentNumbers,
        [Display(Name = "Shipment number {0}: all three parts of the receipt data need to present.")]
        MissingReceiptData,
        [Display(Name = "Shipment number {0}: is duplicated within the data file.")]
        DuplicateShipmentNumber,
        [Display(Name = "Shipment number {0}: data must only be for notification number {1}")]
        WrongNotificationNumber,
        [Display(Name = "Shipment number {0}: the receipt date format must be in dd/mm/yyyy format.")]
        ReceiptDateFormat,
        [Display(Name = "Shipment number {0}: the receipt date must not be in the future and must be equal to or after the Actual Date of Shipment.")]
        ReceiptDateValidation,
        [Display(Name = "Shipment number {0}: the shipment quantity must be numeric.")]
        QuantityNumeric,
        [Display(Name = "Shipment number {0}: the waste quantity must have no more than {1} decimal places.")]
        QuantityPrecision,
        [Display(Name = "Shipment number {0}: the quantity unit of measurement is not permitted on this notification.")]
        QuantityUnit,
        [Display(Name = "Shipment number {0}: the {1} date format must be in dd/mm/yyyy format.")]
        RecoveryDateFormat,
        [Display(Name = "Shipment number {0}: the {1} date must not be in the future and must be equal to or after the date of Receipt.")]
        RecoveryDateValidation,
        [Display(Name = "Shipment number {0}: you can't record Receipt information as the shipment is in an incorrect status or has not been shipped.")]
        PrenotifiedShipment,
        [Display(Name = "Shipment number {0}: you can't record {1} information as the shipment is in an incorrect status or has not been shipped.")]
        RecoveredValidation,
        [Display(Name = "Shipment number {0}: you can't record Receipt and {1} information as the shipment is in an incorrect status or has not been shipped.")]
        ReceivedRecoveredValidation,
        [Display(Name = "Shipment number {0}: this shipment has already been {1}.")]
        AlreadyRecievedRecoveredDisposed
    }
}
