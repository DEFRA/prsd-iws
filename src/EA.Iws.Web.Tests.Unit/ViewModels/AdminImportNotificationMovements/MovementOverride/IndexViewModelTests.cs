namespace EA.Iws.Web.Tests.Unit.ViewModels.AdminImportNotificationMovements.MovementOverride
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Areas.AdminImportNotificationMovements.ViewModels.MovementOverride;
    using Core.ImportMovement;
    using Core.Movement;
    using Core.Shared;
    using Xunit;

    public class IndexViewModelTests
    {
        private readonly DateTime validActualDate = DateTime.UtcNow.Date.AddDays(-5);
        private readonly DateTime validPrenotificationDate = DateTime.UtcNow.Date.AddDays(-8);
        private readonly DateTime validReceivedDate = DateTime.UtcNow.Date.AddDays(-3);
        private readonly DateTime validDisposalDate = DateTime.UtcNow.Date.AddDays(-1);

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
        public void Validate_AcceptedShipment_WithReceivedDateAndNoQuantity_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.ActualQuantity = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("ActualQuantity"));
        }

        [Fact]
        public void Validate_AcceptedShipment_WithoutReceivedDate_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.ReceivedDate = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("ReceivedDate"));
        }

        [Fact]
        public void Validate_AcceptedShipment_WithoutUnits_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.Units = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("Units"));
        }

        [Fact]
        public void Validate_RejectedShipment_WithoutReceivedDate_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Rejected);
            model.ReceivedDate = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("ReceivedDate"));
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
        public void Validate_RejectedShipment_WithZeroRejectedQuantity_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Rejected);
            model.RejectedQuantity = 0;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("RejectedQuantity"));
        }

        [Fact]
        public void Validate_RejectedShipment_WithNegativeRejectedQuantity_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Rejected);
            model.RejectedQuantity = -5;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("RejectedQuantity"));
        }

        [Fact]
        public void Validate_PartiallyRejectedShipment_WithoutReceivedDate_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Partially);
            model.ReceivedDate = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("ReceivedDate"));
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
        public void Validate_PartiallyRejectedShipment_WithZeroActualQuantity_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Partially);
            model.ActualQuantity = 0;

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
        public void Validate_PartiallyRejectedShipment_WithRejectedQuantityGreaterThanActualQuantity_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Partially);
            model.ActualQuantity = 10;
            model.RejectedQuantity = 15;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("RejectedQuantity"));
        }

        [Fact]
        public void Validate_PartiallyRejectedShipment_WithRejectedQuantityEqualToActualQuantity_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Partially);
            model.ActualQuantity = 10;
            model.RejectedQuantity = 10;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("RejectedQuantity"));
        }

        [Fact]
        public void Validate_PartiallyRejectedShipment_WithoutRejectionReason_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Partially);
            model.RejectionReason = null;

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("RejectionReason"));
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
        public void Validate_PrenotificationDateMissing_WhenNotMarkedAsNoPrenotification_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.HasNoPrenotification = false;
            model.PrenotificationDate = null;

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

        // NEW TESTS FOR DISPOSAL/RECOVERY DATE VALIDATION

        [Fact]
        public void Validate_DisposalDateBeforeReceivedDate_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.ReceivedDate = DateTime.UtcNow.Date.AddDays(-3);
            model.Date = DateTime.UtcNow.Date.AddDays(-5); // Disposal date before received date

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("Date"));
            Assert.Contains(results, r => r.ErrorMessage.Contains("recovered") || r.ErrorMessage.Contains("disposed of"));
        }

        [Fact]
        public void Validate_DisposalDateInFuture_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.ReceivedDate = DateTime.UtcNow.Date.AddDays(-3);
            model.Date = DateTime.UtcNow.Date.AddDays(2); // Future disposal date

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("Date"));
            Assert.Contains(results, r => r.ErrorMessage.Contains("future"));
        }

        [Fact]
        public void Validate_DisposalDateEqualToReceivedDate_IsValid()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            var receivedDate = DateTime.UtcNow.Date.AddDays(-3);
            model.ReceivedDate = receivedDate;
            model.Date = receivedDate; // Same day

            var results = ValidateModel(model);

            Assert.DoesNotContain(results, r => r.MemberNames.Contains("Date"));
        }

        [Fact]
        public void Validate_DisposalDateAfterReceivedDate_IsValid()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.ReceivedDate = DateTime.UtcNow.Date.AddDays(-5);
            model.Date = DateTime.UtcNow.Date.AddDays(-2); // Disposal after received

            var results = ValidateModel(model);

            Assert.DoesNotContain(results, r => r.MemberNames.Contains("Date"));
        }

        [Fact]
        public void Validate_DisposalDateToday_IsValid()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.ReceivedDate = DateTime.UtcNow.Date.AddDays(-5);
            model.Date = DateTime.UtcNow.Date; // Today

            var results = ValidateModel(model);

            Assert.DoesNotContain(results, r => r.MemberNames.Contains("Date"));
        }

        [Fact]
        public void Validate_RejectedShipment_WithDisposalDate_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Rejected);
            model.Date = DateTime.UtcNow.Date.AddDays(-1); // Disposal date on rejected shipment

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("Date"));
            Assert.Contains(results, r => r.ErrorMessage.Contains("rejected"));
        }

        [Fact]
        public void Validate_RejectedShipment_WithoutDisposalDate_IsValid()
        {
            var model = CreateValidModel(ShipmentType.Rejected);
            model.Date = null; // No disposal date

            var results = ValidateModel(model);

            // Should not have Date error (other validation errors are expected for rejected shipments)
            Assert.DoesNotContain(results, r => r.MemberNames.Contains("Date"));
        }

        [Fact]
        public void Validate_PartiallyRejectedShipment_WithValidDisposalDate_IsValid()
        {
            var model = CreateValidModel(ShipmentType.Partially);
            model.ReceivedDate = DateTime.UtcNow.Date.AddDays(-5);
            model.Date = DateTime.UtcNow.Date.AddDays(-2); // Valid disposal date

            var results = ValidateModel(model);

            Assert.DoesNotContain(results, r => r.MemberNames.Contains("Date"));
        }

        [Fact]
        public void Validate_PartiallyRejectedShipment_WithDisposalDateBeforeReceivedDate_ReturnsError()
        {
            var model = CreateValidModel(ShipmentType.Partially);
            model.ReceivedDate = DateTime.UtcNow.Date.AddDays(-3);
            model.Date = DateTime.UtcNow.Date.AddDays(-5); // Disposal before received

            var results = ValidateModel(model);

            Assert.Contains(results, r => r.MemberNames.Contains("Date"));
        }

        [Fact]
        public void Validate_DisposalTypeNotification_UsesCorrectVerb()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.NotificationType = NotificationType.Disposal;
            model.ReceivedDate = DateTime.UtcNow.Date.AddDays(-3);
            model.Date = DateTime.UtcNow.Date.AddDays(-5); // Before received date

            var results = ValidateModel(model);

            var dateError = results.FirstOrDefault(r => r.MemberNames.Contains("Date"));
            Assert.NotNull(dateError);
            Assert.Contains("disposed of", dateError.ErrorMessage);
        }

        [Fact]
        public void Validate_RecoveryTypeNotification_UsesCorrectVerb()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.NotificationType = NotificationType.Recovery;
            model.ReceivedDate = DateTime.UtcNow.Date.AddDays(-3);
            model.Date = DateTime.UtcNow.Date.AddDays(-5); // Before received date

            var results = ValidateModel(model);

            var dateError = results.FirstOrDefault(r => r.MemberNames.Contains("Date"));
            Assert.NotNull(dateError);
            Assert.Contains("recovered", dateError.ErrorMessage);
        }

        [Fact]
        public void Validate_NoDisposalDate_DoesNotTriggerDateValidation()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.Date = null;

            var results = ValidateModel(model);

            Assert.DoesNotContain(results, r => r.MemberNames.Contains("Date"));
        }

        [Fact]
        public void Validate_CompletelyValidAcceptedShipmentWithDisposalDate_ReturnsNoErrors()
        {
            var model = CreateValidModel(ShipmentType.Accepted);
            model.ReceivedDate = DateTime.UtcNow.Date.AddDays(-5);
            model.Date = DateTime.UtcNow.Date.AddDays(-2);
            model.ActualQuantity = 10;
            model.Units = ShipmentQuantityUnits.Tonnes;

            var results = ValidateModel(model);

            Assert.Empty(results);
        }

        [Fact]
        public void Validate_CompletelyValidPartiallyRejectedShipmentWithDisposalDate_ReturnsNoErrors()
        {
            var model = CreateValidModel(ShipmentType.Partially);
            model.ReceivedDate = DateTime.UtcNow.Date.AddDays(-5);
            model.Date = DateTime.UtcNow.Date.AddDays(-2);
            model.ActualQuantity = 10;
            model.RejectedQuantity = 3;

            var results = ValidateModel(model);

            Assert.Empty(results);
        }

        private ImportMovementSummaryData CreateMovementData(bool isReceived, bool isRejected, bool isPartiallyRejected)
        {
            return new ImportMovementSummaryData
            {
                MovementId = Guid.NewGuid(),
                Data = new ImportMovementData
                {
                    NotificationId = Guid.NewGuid(),
                    Number = 1,
                    ActualDate = validActualDate,
                    PreNotificationDate = validPrenotificationDate,
                    NotificationType = NotificationType.Recovery
                },
                ReceiptData = new ImportMovementReceiptData
                {
                    IsReceived = isReceived,
                    IsRejected = isRejected,
                    PossibleUnits = new List<ShipmentQuantityUnits> { ShipmentQuantityUnits.Tonnes }
                },
                RecoveryData = new ImportMovementRecoveryData(),
                IsPartiallyRejected = isPartiallyRejected,
                IsReceived = isReceived,
                IsRejected = isRejected
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