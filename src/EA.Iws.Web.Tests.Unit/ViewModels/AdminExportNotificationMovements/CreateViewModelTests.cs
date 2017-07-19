namespace EA.Iws.Web.Tests.Unit.ViewModels.AdminExportNotificationMovements
{
    using System;
    using EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement;
    using Prsd.Core;
    using TestHelpers;
    using Web.ViewModels.Shared;
    using Xunit;

    public class CreateViewModelTests : IDisposable
    {
        public CreateViewModelTests()
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
            var model = CreateTestModel(30, 5, 2016, "PRE_NOTIFICATION");
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void PrenotificationDateCannotBeInTheFuture()
        {
            var model = CreateTestModel(30, 7, 2016, "PRE_NOTIFICATION");           
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void PrenotificationDateCanBeToday()
        {
            var model = CreateTestModel(1, 6, 2016, "PRE_NOTIFICATION");           
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void ActualShipmentDateCanBeInThePast()
        {
            var model = CreateTestModel(15, 6, 2016, "ACTUALDATE_PAST");
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void ActualShipmentDateCanBeToday()
        {
            var model = CreateTestModel(1, 6, 2016, "ACTUALDATE");
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void ActualShipmentDateNotBeforePrenotificationDate()
        {
            var model = CreateTestModel(31, 5, 2016, "ACTUALDATE");
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void ActualShipmentDateSameasPrenotificationDate()
        {
            var model = CreateTestModel(1, 6, 2016, "ACTUALDATE");
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }       

        [Fact]
        public void ActualShipmentDateCanBeSixtyDaysAfterPrenotification()
        {
            var model = CreateTestModel(31, 7, 2016, "ACTUALDATE");
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void ActualShipmentDateGreaterThanSixtyDaysAfterPrenotification()
        {
            var model = CreateTestModel(2, 8, 2016, "ACTUALDATE");
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteReceivedDateCanBeInThePast()
        {
            var model = CreateTestModel(30, 5, 2016, "RECEIVED_PAST");
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteReceivedDateCanBeToday()
        {
            var model = CreateTestModel(1, 6, 2016, "RECEIVED");
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteReceivedDateNotBeforeActualShipmentDate()
        {
            var model = CreateTestModel(31, 5, 2016, "RECEIVED");
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteReceivedDateSameasActualShipmentDate()
        {
            var model = CreateTestModel(1, 6, 2016, "RECEIVED");
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteReceivedDateCannotBeInTheFuture()
        {
            var model = CreateTestModel(30, 7, 2016, "RECEIVED");
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteRecoveredDateCanBeInThePast()
        {
            var model = CreateTestModel(30, 5, 2016, "RECOVERED_PAST");
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteRecoveredDateCanBeToday()
        {
            var model = CreateTestModel(1, 6, 2016, "RECOVERED");
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteRecoveredDateNotBeforeWasteReceivedDate()
        {
            var model = CreateTestModel(31, 5, 2016, "RECOVERED");
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteRecoveredDateSameasWasteReceivedDate()
        {
            var model = CreateTestModel(1, 6, 2016, "RECOVERED");
            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void WasteRecoveredDateCannotBeInTheFuture()
        {
            var model = CreateTestModel(30, 7, 2016, "RECOVERED");
            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="day"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="type">
        /// PRE_NOTIFICATION - Prenotification Date
        /// A - Actual shipment Date
        /// RECEIVED - Waste received Date
        /// RECOVERED - Waste recovered Date
        /// </param>
        /// <returns></returns>
        internal CreateViewModel CreateTestModel(int day, int month, int year, string type)
        {
            var model = new CreateViewModel();
            model.Number = 52;
            if (type.Equals("PRE_NOTIFICATION"))
            {
                model.PrenotificationDate = new OptionalDateInputViewModel(new DateTime(year, month, day));
                model.ActualShipmentDate = new OptionalDateInputViewModel(new DateTime(year, month, day));
            }
            else if (type.Equals("ACTUALDATE"))
            {
                model.PrenotificationDate = new OptionalDateInputViewModel(new DateTime(2016, 6, 1));
                model.ActualShipmentDate = new OptionalDateInputViewModel(new DateTime(year, month, day));
            }           
            else if (type.Equals("ACTUALDATE_PAST"))
            {
                model.PrenotificationDate = new OptionalDateInputViewModel(new DateTime(2016, 6, 1));
                model.ActualShipmentDate = new OptionalDateInputViewModel(new DateTime(year, month, day));
            }
            else if (type.Equals("RECEIVED"))
            {
                model.PrenotificationDate = new OptionalDateInputViewModel(new DateTime(2016, 6, 1));
                model.ActualShipmentDate = new OptionalDateInputViewModel(new DateTime(2016, 6, 1));
                model.Receipt.ReceivedDate = new OptionalDateInputViewModel(new DateTime(year, month, day));
                model.Receipt.ActualQuantity = 10;
                model.Receipt.WasShipmentAccepted = true;
                model.Receipt.Units = Core.Shared.ShipmentQuantityUnits.Tonnes;
            }
            else if (type.Equals("RECEIVED_PAST"))
            {
                model.PrenotificationDate = new OptionalDateInputViewModel(new DateTime(2016, 5, 1));
                model.ActualShipmentDate = new OptionalDateInputViewModel(new DateTime(2016, 5, 15));
                model.Receipt.ReceivedDate = new OptionalDateInputViewModel(new DateTime(year, month, day));
                model.Receipt.ActualQuantity = 10;
                model.Receipt.WasShipmentAccepted = true;
                model.Receipt.Units = Core.Shared.ShipmentQuantityUnits.Tonnes;
            }
            else if (type.Equals("RECOVERED"))
            {
                model.PrenotificationDate = new OptionalDateInputViewModel(new DateTime(2016, 6, 1));
                model.ActualShipmentDate = new OptionalDateInputViewModel(new DateTime(2016, 6, 1));
                model.Receipt.ReceivedDate = new OptionalDateInputViewModel(new DateTime(2016, 6, 1));
                model.Receipt.ActualQuantity = 10;
                model.Receipt.WasShipmentAccepted = true;
                model.Receipt.Units = Core.Shared.ShipmentQuantityUnits.Tonnes;
                model.Recovery.RecoveryDate = new OptionalDateInputViewModel(new DateTime(year, month, day));
            }
            else if (type.Equals("RECOVERED_PAST"))
            {
                model.PrenotificationDate = new OptionalDateInputViewModel(new DateTime(2016, 5, 1));
                model.ActualShipmentDate = new OptionalDateInputViewModel(new DateTime(2016, 5, 1));
                model.Receipt.ReceivedDate = new OptionalDateInputViewModel(new DateTime(2016, 5, 1));
                model.Receipt.ActualQuantity = 10;
                model.Receipt.WasShipmentAccepted = true;
                model.Receipt.Units = Core.Shared.ShipmentQuantityUnits.Tonnes;
                model.Recovery.RecoveryDate = new OptionalDateInputViewModel(new DateTime(year, month, day));
            }

            return model;
        }
    }
}
