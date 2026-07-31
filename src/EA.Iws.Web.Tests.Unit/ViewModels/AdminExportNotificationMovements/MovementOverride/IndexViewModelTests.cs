namespace EA.Iws.Web.Tests.Unit.ViewModels.AdminExportNotificationMovements.MovementOverride
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Areas.AdminExportNotificationMovements.ViewModels.MovementOverride;
    using Core.Movement;
    using Core.Shared;
    using Xunit;

    public class IndexViewModelTests
    {
        private readonly DateTime validActualDate = DateTime.UtcNow.Date.AddDays(-5);
        private readonly DateTime validPrenotificationDate = DateTime.UtcNow.Date.AddDays(-8);
        private readonly DateTime validReceivedDate = DateTime.UtcNow.Date.AddDays(-3);

        [Fact]
        public void Constructor_WithReceivedData_SetsShipmentTypesToAccepted()
        {
            var data = CreateMovementData(isReceived: true, isRejected: false, isPartiallyRejected: false);

            var model = new IndexViewModel(data);

            Assert.Equal(ShipmentType.Accepted, model.ShipmentTypes);
        }

        [Fact]
        public void Constructor_WithRejectedData_SetsShipmentTypesToRejected()
        {
            var data = CreateMovementData(isReceived: false, isRejected: true, isPartiallyRejected: false);

            var model = new IndexViewModel(data);

            Assert.Equal(ShipmentType.Rejected, model.ShipmentTypes);
        }

        [Fact]
        public void Constructor_WithPartiallyRejectedData_SetsShipmentTypesToPartially()
        {
            var data = CreateMovementData(isReceived: false, isRejected: false, isPartiallyRejected: true);

            var model = new IndexViewModel(data);

            Assert.Equal(ShipmentType.Partially, model.ShipmentTypes);
        }

        [Fact]
        public void Constructor_WithNoStatus_DefaultsToAccepted()
        {
            var data = CreateMovementData(isReceived: false, isRejected: false, isPartiallyRejected: false);

            var model = new IndexViewModel(data);

            Assert.Equal(ShipmentType.Accepted, model.ShipmentTypes);
        }

        [Fact]
        public void Validate_AcceptedShipment_WithoutActualQuantity_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.ActualQuantity = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("ActualQuantity"));
        }

        [Fact]
        public void Validate_RejectedShipment_WithoutRejectionReason_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Rejected);
            model.RejectionReason = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("RejectionReason"));
        }

        [Fact]
        public void Validate_RejectedShipment_WithoutRejectedQuantity_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Rejected);
            model.RejectedQuantity = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("RejectedQuantity"));
        }

        [Fact]
        public void Validate_RejectedShipment_WithoutStatsMarking_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Rejected);
            model.StatsMarking = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("StatsMarking"));
        }

        [Fact]
        public void Validate_PartiallyRejectedShipment_WithoutActualQuantity_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Partially);
            model.ActualQuantity = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("ActualQuantity"));
        }

        [Fact]
        public void Validate_PartiallyRejectedShipment_WithoutRejectedQuantity_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Partially);
            model.RejectedQuantity = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("RejectedQuantity"));
        }

        [Fact]
        public void Validate_PartiallyRejectedShipment_WithoutStatsMarking_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Partially);
            model.StatsMarking = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("StatsMarking"));
        }

        [Fact]
        public void Validate_PrenotificationDateInFuture_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.PrenotificationDate = DateTime.UtcNow.Date.AddDays(5);

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("PrenotificationDate"));
        }

        [Fact]
        public void Validate_NoPrenotificationAndNoFlag_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.PrenotificationDate = null;
            model.HasNoPrenotification = false;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("PrenotificationDate"));
        }

        [Fact]
        public void Validate_ActualShipmentDateMissing_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.ActualShipmentDate = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("ActualShipmentDate"));
        }

        private MovementReceiptAndRecoveryData CreateMovementData(bool isReceived, bool isRejected, bool isPartiallyRejected)
        {
            return new MovementReceiptAndRecoveryData
            {
                Id = Guid.NewGuid(),
                NotificationId = Guid.NewGuid(),
                Number = 1,
                ActualDate = validActualDate,
                PrenotificationDate = validPrenotificationDate,
                NotificationType = NotificationType.Recovery,
                IsReceived = isReceived,
                IsRejected = isRejected,
                IsPartiallyRejected = isPartiallyRejected,
                PossibleUnits = new List<ShipmentQuantityUnits> { ShipmentQuantityUnits.Tonnes }
            };
        }

        private IndexViewModel CreateValidModel(ShipmentType shipmentType)
        {
            var model = new IndexViewModel
            {
                ShipmentNumber = 1,
                NotificationType = NotificationType.Recovery,
                ActualShipmentDate = validActualDate,
                PrenotificationDate = validPrenotificationDate,
                ReceivedDate = validReceivedDate,
                ShipmentTypes = shipmentType,
                ActualQuantity = 10,
                Units = ShipmentQuantityUnits.Tonnes,
                PossibleUnits = new List<ShipmentQuantityUnits> { ShipmentQuantityUnits.Tonnes }
            };

            if (shipmentType == ShipmentType.Rejected || shipmentType == ShipmentType.Partially)
            {
                model.RejectionReason = "Test rejection reason";
                model.RejectedQuantity = 5;
                model.RejectedUnits = ShipmentQuantityUnits.Tonnes;
                model.StatsMarking = "Illegal";
            }

            return model;
        }

        private IList<ValidationResult> ValidateModel(IndexViewModel model)
        {
            var context = new ValidationContext(model);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, context, results, true);
            results.AddRange(model.Validate(context));
            return results;
        }
    }
}