namespace EA.Iws.DocumentGeneration.Tests.Unit.ViewModels
{
    using System;
    using Core.Shared;
    using DocumentGeneration.Formatters;
    using DocumentGeneration.ViewModels;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class MovementViewModelTests
    {
        private readonly TestableMovement movement;
        private readonly TestableNotificationApplication notification;

        public MovementViewModelTests()
        {
            notification = new TestableNotificationApplication
            {
                Id = new Guid("07FF7B1D-A3A9-4FB1-B10B-F00EEF8FB9F8")
            };
            movement = new TestableMovement
            {
                Id = new Guid("B20B7BBE-EA7C-45F7-B6DB-04F645E7375B")
            };
        }

        [Fact]
        public void MovementIsNull()
        {
            var result = new MovementViewModel(null, new DateTimeFormatter(), new QuantityFormatter(),
                new PhysicalCharacteristicsFormatter());

            new ExpectedMovementViewModel
            {
                ActualCubicMetres = string.Empty,
                ActualDate = string.Empty,
                ActualKilograms = string.Empty,
                ActualLitres = string.Empty,
                ActualTonnes = string.Empty,
                IsNotSpecialHandling = false,
                IsSpecialHandling = false,
                NotificationNumber = string.Empty,
                Number = string.Empty,
                PhysicalCharacteristics = string.Empty
            }.Evaluate(result);
        }

        [Fact]
        public void SetsNotificationProperties()
        {
            notification.NotificationNumber = "GB 001 00250";
            notification.HasSpecialHandlingRequirements = true;
            notification.PhysicalCharacteristics = new[]
            {
                PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(PhysicalCharacteristicType.Sludgy)
            };
            movement.NotificationApplication = notification;

            var result = GenerateViewModel();

            new ExpectedMovementViewModel
            {
                ActualCubicMetres = string.Empty,
                ActualDate = string.Empty,
                ActualKilograms = string.Empty,
                ActualLitres = string.Empty,
                ActualTonnes = string.Empty,
                IsNotSpecialHandling = false,
                IsSpecialHandling = true,
                NotificationNumber = notification.NotificationNumber,
                Number = "0",
                PhysicalCharacteristics = "4"
            }.Evaluate(result);
        }

        [Fact]
        public void SetsMovementProperties()
        {
            movement.Date = new DateTime(2015, 1, 1);
            movement.Quantity = 70;
            movement.Units = ShipmentQuantityUnits.Kilograms;
            movement.DisplayUnits = ShipmentQuantityUnits.Kilograms;
            movement.Number = 5;

            var result = GenerateViewModel();

            new ExpectedMovementViewModel
            {
                ActualCubicMetres = string.Empty,
                ActualDate = "01.01.15",
                ActualKilograms = "70 kg",
                ActualLitres = string.Empty,
                ActualTonnes = string.Empty,
                IsNotSpecialHandling = false,
                IsSpecialHandling = false,
                NotificationNumber = string.Empty,
                Number = "5",
                PhysicalCharacteristics = string.Empty
            }.Evaluate(result);
        }

        [Theory]
        [InlineData(70, ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.Litres, "", "", "70 Ltrs", "")]
        [InlineData(70, ShipmentQuantityUnits.CubicMetres, ShipmentQuantityUnits.CubicMetres, "70", "", "", "")]
        [InlineData(70, ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.Tonnes, "", "", "", "70")]
        [InlineData(70, ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Kilograms, "", "70 kg", "", "")]
        [InlineData(70.25, ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Kilograms, "", "70.25 kg", "", "")]
        [InlineData(70.250, ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Kilograms, "", "70.25 kg", "", "")]
        public void SetsMovementPropertiesWithCorrectUnits(decimal quantity,
            ShipmentQuantityUnits units,
            ShipmentQuantityUnits displayUnits,
            string cubicDisplay,
            string kilogramsDisplay,
            string litresDisplay,
            string tonnesDisplay)
        {
            movement.Date = new DateTime(2015, 12, 15);
            movement.Quantity = quantity;
            movement.Units = units;
            movement.DisplayUnits = displayUnits;
            movement.Number = 7;

            var result = GenerateViewModel();

            new ExpectedMovementViewModel
            {
                ActualCubicMetres = cubicDisplay,
                ActualDate = "15.12.15",
                ActualKilograms = kilogramsDisplay,
                ActualLitres = litresDisplay,
                ActualTonnes = tonnesDisplay,
                IsNotSpecialHandling = false,
                IsSpecialHandling = false,
                NotificationNumber = string.Empty,
                Number = "7",
                PhysicalCharacteristics = string.Empty
            }.Evaluate(result);
        }

        [Fact]
        public void SetsMovementPropertiesConvertingToDisplayUnits()
        {
            movement.Date = new DateTime(2010, 10, 5);
            movement.Quantity = 69;
            movement.Units = ShipmentQuantityUnits.Tonnes;
            movement.DisplayUnits = ShipmentQuantityUnits.Kilograms;
            movement.Number = 2;

            var result = GenerateViewModel();

            new ExpectedMovementViewModel
            {
                ActualCubicMetres = string.Empty,
                ActualDate = "05.10.10",
                ActualKilograms = "69000 kg",
                ActualLitres = string.Empty,
                ActualTonnes = string.Empty,
                IsNotSpecialHandling = false,
                IsSpecialHandling = false,
                NotificationNumber = string.Empty,
                Number = "2",
                PhysicalCharacteristics = string.Empty
            }.Evaluate(result);
        }

        [Fact]
        public void SetsAllProperties()
        {
            movement.Date = new DateTime(2025, 5, 2);
            movement.Quantity = 30.7m;
            movement.Units = ShipmentQuantityUnits.Kilograms;
            movement.DisplayUnits = ShipmentQuantityUnits.Tonnes;
            movement.Number = 12;

            notification.NotificationNumber = "GB 001 00250";
            notification.HasSpecialHandlingRequirements = false;
            notification.PhysicalCharacteristics = new[]
            {
                PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(PhysicalCharacteristicType.Sludgy),
                PhysicalCharacteristicsInfo.CreateOtherPhysicalCharacteristicsInfo("Pastel"),
                PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(PhysicalCharacteristicType.Powdery)
            };

            movement.NotificationApplication = notification;

            var result = GenerateViewModel();

            new ExpectedMovementViewModel
            {
                ActualCubicMetres = string.Empty,
                ActualDate = "02.05.25",
                ActualKilograms = string.Empty,
                ActualLitres = string.Empty,
                ActualTonnes = "0.0307",
                IsNotSpecialHandling = true,
                IsSpecialHandling = false,
                NotificationNumber = notification.NotificationNumber,
                Number = "12",
                PhysicalCharacteristics = "1, 4, Pastel"
            }.Evaluate(result);
        }

        private MovementViewModel GenerateViewModel()
        {
            return new MovementViewModel(movement,
                new DateTimeFormatter(),
                new QuantityFormatter(),
                new PhysicalCharacteristicsFormatter());
        }

        private class ExpectedMovementViewModel
        {
            public string ActualCubicMetres { get; set; }
            public string ActualDate { get; set; }
            public string ActualKilograms { get; set; }
            public string ActualLitres { get; set; }
            public string ActualTonnes { get; set; }
            public string NotificationNumber { get; set; }
            public string Number { get; set; }
            public string PhysicalCharacteristics { get; set; }
            public bool IsNotSpecialHandling { get; set; }
            public bool IsSpecialHandling { get; set; }

            public void Evaluate(MovementViewModel model)
            {
                Assert.Equal(ActualCubicMetres, model.ActualCubicMetres);
                Assert.Equal(ActualDate, model.ActualDate);
                Assert.Equal(ActualKilograms, model.ActualKilograms);
                Assert.Equal(ActualLitres, model.ActualLitres);
                Assert.Equal(ActualTonnes, model.ActualTonnes);
                Assert.Equal(NotificationNumber, model.NotificationNumber);
                Assert.Equal(Number, model.Number);
                Assert.Equal(PhysicalCharacteristics, model.PhysicalCharacteristics);
                Assert.Equal(IsNotSpecialHandling, model.IsNotSpecialHandling);
                Assert.Equal(IsSpecialHandling, model.IsSpecialHandling);
            }
        }
    }
}
