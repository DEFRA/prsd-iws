namespace EA.Iws.Web.Tests.Unit.ViewModels.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Areas.Movement.ViewModels.Carrier;
    using Core.Carriers;
    using TestHelpers;
    using Xunit;

    public class CarrierViewModelTests
    {
        private static readonly Guid Carrier1Id = new Guid("37794D13-3633-4399-A1BC-A1D45F7BB80B");
        private static readonly Guid Carrier2Id = new Guid("B054A03C-11E5-4D20-A577-500A50BAABDD");

        private readonly IEnumerable<CarrierData> notificationCarriers;
        private readonly Dictionary<int, CarrierData> selectedCarriers;
        private readonly CarrierData carrier1;
        private readonly CarrierData carrier2;

        public CarrierViewModelTests()
        {
            carrier1 = new CarrierData { Id = Carrier1Id };
            carrier2 = new CarrierData { Id = Carrier2Id };

            notificationCarriers = new[] 
            { 
                new CarrierData { Id = Carrier1Id }, 
                new CarrierData { Id = Carrier2Id } 
            };

            selectedCarriers = new Dictionary<int, CarrierData> { { 0, carrier1 } };
        }

        [Fact]
        public void SelectedCarriersEmpty_SelectedItemsNullAndSizeOf_NumberOfCarriers()
        {
            var model = GetViewModel(notificationCarriers);

            model.SetCarrierSelectLists(new Dictionary<int, CarrierData>(), 3);

            Assert.Equal(3, model.SelectedItems.Count);
            Assert.Collection(model.SelectedItems,
                item => Assert.Null(model.SelectedItems[0]),
                item => Assert.Null(model.SelectedItems[1]),
                item => Assert.Null(model.SelectedItems[2]));
        }

        [Fact]
        public void CarrierSelectLists_SameCountAsNumberOfCarriers()
        {
            var model = GetViewModel(notificationCarriers);

            model.SetCarrierSelectLists(new Dictionary<int, CarrierData>(), 2);

            Assert.Equal(2, model.CarrierSelectLists.Count);
        }

        [Fact]
        public void CarrierSelectLists_OrderIsZeroIndexed()
        {
            var model = GetViewModel(notificationCarriers);

            model.SetCarrierSelectLists(new Dictionary<int, CarrierData>(), 2);

            Assert.Equal(0, model.CarrierSelectLists.Keys.ElementAt(0));
            Assert.Equal(1, model.CarrierSelectLists.Keys.ElementAt(1));
        }

        [Fact]
        public void SelectedItems_ContainsGuid_IfSelectedCarrierExists()
        {
            var model = GetViewModel(notificationCarriers);

            model.SetCarrierSelectLists(selectedCarriers, 1);

            Assert.Equal(Carrier1Id, model.SelectedItems.Single());
        }

        [Fact]
        public void SelectedItems_ContainsGuidAndNull_IfCarrierExists_AndMoreCarriersSpecified()
        {
            var model = GetViewModel(notificationCarriers);

            model.SetCarrierSelectLists(selectedCarriers, 2);

            Assert.Equal(2, model.SelectedItems.Count);
            Assert.Equal(Carrier1Id, model.SelectedItems[0]);
            Assert.Null(model.SelectedItems[1]);
        }

        [Fact]
        public void SelectedItems_ContainsOnlyOneGuid_IfTwoCarriersExist_AndOneCarrierIsSpecified()
        {
            var model = GetViewModel(notificationCarriers);

            var twoSelectedCarriers = new Dictionary<int, CarrierData> { { 0, carrier1 }, { 1, carrier2 } };

            model.SetCarrierSelectLists(twoSelectedCarriers, 1);

            Assert.Equal(1, model.SelectedItems.Count);
            Assert.Equal(Carrier1Id, model.SelectedItems[0]);
        }

        [Fact]
        public void Validate_AllowsDifferentSelectedCarriers()
        {
            var model = GetViewModel(notificationCarriers);

            model.SelectedItems = new List<Guid?> { Carrier1Id, Carrier2Id };

            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void Validate_DoesNotAllowConsecutiveCarriersSame()
        {
            var model = GetViewModel(notificationCarriers);

            model.SelectedItems = new List<Guid?> { Carrier1Id, Carrier1Id };

            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void Validate_AllowsMultipleOfSameCarrierIfNotConsecutive()
        {
            var model = GetViewModel(notificationCarriers);

            model.SelectedItems = new List<Guid?> { Carrier1Id, Carrier2Id, Carrier1Id };

            Assert.Empty(ViewModelValidator.ValidateViewModel(model));
        }

        [Fact]
        public void Validate_DoesNotAllowEmptySelection()
        {
            var model = GetViewModel(notificationCarriers);

            model.SelectedItems = new List<Guid?> { null, Carrier1Id };

            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(model));
        }

        private CarrierViewModel GetViewModel(IEnumerable<CarrierData> notificationCarriers)
        {
            return new CarrierViewModel
            {
                NotificationCarriers = notificationCarriers.ToList()
            };
        }
    }
}
