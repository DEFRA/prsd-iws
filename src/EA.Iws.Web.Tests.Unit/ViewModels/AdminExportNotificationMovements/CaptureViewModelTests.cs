namespace EA.Iws.Web.Tests.Unit.ViewModels.AdminExportNotificationMovements
{
    using System;
    using Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement;
    using Prsd.Core;
    using TestHelpers;
    using Web.ViewModels.Shared;
    using Xunit;

    public class CaptureViewModelTests : IDisposable
    {
        public CaptureViewModelTests()
        {
            SystemTime.Freeze(new DateTime(2016, 6, 1));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public void PrenotificationDateCanBeInThePast()
        {
            var model = CreateViewModelForPrenotificationDate(30, 5, 2016);
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void PrenotificationDateCannotBeInTheFuture()
        {
            var model = CreateViewModelForPrenotificationDate(30, 7, 2016);           
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void PrenotificationDateCanBeToday()
        {
            var model = CreateViewModelForPrenotificationDate(1, 6, 2016);           
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void ActualShipmentDateCanBeInThePast()
        {
            var model = CreateViewModelForActualDate(15, 5, 2016, true);
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void ActualShipmentDateCanBeToday()
        {
            var model = CreateViewModelForActualDate(1, 6, 2016, false);
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void ActualShipmentDateNotBeforePrenotificationDate()
        {
            var model = CreateViewModelForActualDate(31, 5, 2016, false);
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void ActualShipmentDateSameasPrenotificationDate()
        {
            var model = CreateViewModelForActualDate(1, 6, 2016, false);
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }       

        [Fact]
        public void ActualShipmentDateCanBeSixtyDaysAfterPrenotification()
        {
            var model = CreateViewModelForActualDate(31, 7, 2016, false);
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void ActualShipmentDateGreaterThanSixtyDaysAfterPrenotification()
        {
            var model = CreateViewModelForActualDate(2, 8, 2016, false);
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteReceivedDateCanBeInThePast()
        {
            var model = CreateViewModelForReceivedDate(30, 5, 2016, true);
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteReceivedDateCanBeToday()
        {
            var model = CreateViewModelForReceivedDate(1, 6, 2016, false);
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteReceivedDateNotBeforeActualShipmentDate()
        {
            var model = CreateViewModelForReceivedDate(31, 5, 2016, false);
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteReceivedDateSameasActualShipmentDate()
        {
            var model = CreateViewModelForReceivedDate(1, 6, 2016, false);
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteReceivedDateCannotBeInTheFuture()
        {
            var model = CreateViewModelForReceivedDate(30, 7, 2016, false);
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteRecoveredDateCanBeInThePast()
        {
            var model = CreateViewModelForRecoveredDate(30, 5, 2016, true);
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteRecoveredDateCanBeToday()
        {
            var model = CreateViewModelForRecoveredDate(1, 6, 2016, false);
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteRecoveredDateNotBeforeWasteReceivedDate()
        {
            var model = CreateViewModelForRecoveredDate(31, 5, 2016, false);
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteRecoveredDateSameasWasteReceivedDate()
        {
            var model = CreateViewModelForRecoveredDate(1, 6, 2016, false);
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteRecoveredDateCannotBeInTheFuture()
        {
            var model = CreateViewModelForRecoveredDate(30, 7, 2016, false);
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        internal CaptureViewModel CreateViewModelForPrenotificationDate(int day, int month, int year)
        {
            var model = new CaptureViewModel();
            model.ShipmentNumber = 52;
            model.PrenotificationDate = new MaskedDateInputViewModel(new DateTime(year, month, day));
            model.ActualShipmentDate = new MaskedDateInputViewModel(new DateTime(year, month, day));
            return model;
        }
        internal CaptureViewModel CreateViewModelForActualDate(int day, int month, int year, bool isDateInPast)
        {
            var model = new CaptureViewModel();
            model.ShipmentNumber = 52;

            if (!isDateInPast)
            {
                model.PrenotificationDate = new MaskedDateInputViewModel(new DateTime(2016, 6, 1));
                model.ActualShipmentDate = new MaskedDateInputViewModel(new DateTime(year, month, day));
            }
            else
            {
                model.PrenotificationDate = new MaskedDateInputViewModel(new DateTime(2016, 5, 1));
                model.ActualShipmentDate = new MaskedDateInputViewModel(new DateTime(year, month, day));
            }
            return model;
        }

        internal CaptureViewModel CreateViewModelForReceivedDate(int day, int month, int year, bool isDateInPast)
        {
            var model = new CaptureViewModel();
            model.ShipmentNumber = 52;

            if (!isDateInPast)
            {
                model.PrenotificationDate = new MaskedDateInputViewModel(new DateTime(2016, 6, 1));
                model.ActualShipmentDate = new MaskedDateInputViewModel(new DateTime(2016, 6, 1));             
            }
            else
            {
                model.PrenotificationDate = new MaskedDateInputViewModel(new DateTime(2016, 5, 1));
                model.ActualShipmentDate = new MaskedDateInputViewModel(new DateTime(2016, 5, 15));
            }

            model.Receipt.ReceivedDate = new MaskedDateInputViewModel(new DateTime(year, month, day));
            model.Receipt.ActualQuantity = 10;
            model.Receipt.WasShipmentAccepted = true;
            model.Receipt.Units = Core.Shared.ShipmentQuantityUnits.Tonnes;

            return model;
        }

        internal CaptureViewModel CreateViewModelForRecoveredDate(int day, int month, int year, bool isDateInPast)
        {
            var model = new CaptureViewModel();
            model.ShipmentNumber = 52;
            if (!isDateInPast)
            {
                model.PrenotificationDate = new MaskedDateInputViewModel(new DateTime(2016, 6, 1));
                model.ActualShipmentDate = new MaskedDateInputViewModel(new DateTime(2016, 6, 1));
                model.Receipt.ReceivedDate = new MaskedDateInputViewModel(new DateTime(2016, 6, 1));               
            }
            else 
            {
                model.PrenotificationDate = new MaskedDateInputViewModel(new DateTime(2016, 5, 1));
                model.ActualShipmentDate = new MaskedDateInputViewModel(new DateTime(2016, 5, 1));
                model.Receipt.ReceivedDate = new MaskedDateInputViewModel(new DateTime(2016, 5, 1));
            }
            model.Receipt.ActualQuantity = 10;
            model.Receipt.WasShipmentAccepted = true;
            model.Receipt.Units = Core.Shared.ShipmentQuantityUnits.Tonnes;
            model.Recovery.RecoveryDate = new MaskedDateInputViewModel(new DateTime(year, month, day));
            return model;
        }
    }
}
