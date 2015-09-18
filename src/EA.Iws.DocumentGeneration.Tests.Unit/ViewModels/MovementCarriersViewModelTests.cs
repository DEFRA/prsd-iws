namespace EA.Iws.DocumentGeneration.Tests.Unit.ViewModels
{
    using System.Collections.Generic;
    using DocumentGeneration.ViewModels;
    using Domain.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class MovementCarriersViewModelTests
    {
        [Fact]
        public void CreateViewModelSucceeds()
        {
            var carriers = GetCarriersList();
            var vm = new MovementCarriersViewModel(carriers);

            AssertFirstCarrierIsDefault(vm);
            AssertSecondCarrierIsEmpty(vm);
            AssertLastCarrierIsEmpty(vm);
        }

        [Fact]
        public void OneCarrier_FirstFieldsCompleted()
        {
            var carriers = GetCarriersList();
            var vm = new MovementCarriersViewModel(carriers);

            AssertSecondCarrierIsEmpty(vm);
            AssertLastCarrierIsEmpty(vm);
            Assert.Equal(string.Empty, vm.AnnexMessage);
        }

        [Fact]
        public void TwoCarriers_FirstAndLastFieldsCompleted()
        {
            var carriers = GetCarriersList();

            var c = CreateMovementCarrierWithCarrierPosition(2);
            carriers.Add(c);

            var vm = new MovementCarriersViewModel(carriers);

            AssertFirstCarrierIsDefault(vm);
            AssertSecondCarrierIsEmpty(vm);
            AssertLastCarrierIsDefault(vm);
            Assert.Equal(string.Empty, vm.AnnexMessage);
        }

        [Fact]
        public void ThreeCarriers_AllFieldsCompleted()
        {
            var carriers = GetCarriersList();

            var c2 = CreateMovementCarrierWithCarrierPosition(2);
            carriers.Add(c2);

            var c3 = CreateMovementCarrierWithCarrierPosition(3);
            carriers.Add(c3);

            var vm = new MovementCarriersViewModel(carriers);

            AssertFirstCarrierIsDefault(vm);
            AssertSecondCarrierIsDefault(vm);
            AssertLastCarrierIsDefault(vm);
            Assert.Equal(string.Empty, vm.AnnexMessage);
        }

        [Fact]
        public void MoreThenThreeCarriersFieldsAreEmptyListIsPopulated()
        {
            var carriers = GetCarriersList();

            var c2 = CreateMovementCarrierWithCarrierPosition(2);
            carriers.Add(c2);

            var c3 = CreateMovementCarrierWithCarrierPosition(3);
            carriers.Add(c3);

            var c4 = CreateMovementCarrierWithCarrierPosition(4);
            carriers.Add(c4);

            var vm = new MovementCarriersViewModel(carriers);

            AssertFirstCarrierIsEmpty(vm);
            AssertSecondCarrierIsEmpty(vm);
            AssertLastCarrierIsEmpty(vm);
            AssertCarrierDetailsSet(vm);
            Assert.Equal("See carriers annex", vm.AnnexMessage);
        }

        private MovementCarrier CreateMovementCarrierWithCarrierPosition(int order)
        {
            var carrier = new TestableCarrier
            {
                Address = TestableAddress.SouthernHouse,
                Business = TestableBusiness.WasteSolutions,
                Contact = TestableContact.BillyKnuckles
            };

            return new MovementCarrier(order, carrier);
        }

        private List<MovementCarrier> GetCarriersList()
        {
            var carrier = new TestableCarrier
            {
                Address = TestableAddress.SouthernHouse,
                Business = TestableBusiness.WasteSolutions,
                Contact = TestableContact.BillyKnuckles
            };

            var c = new MovementCarrier(1, carrier);
            var carriers = new List<MovementCarrier>();
            carriers.Add(c);

            return carriers;
        }

        private void AssertFirstCarrierIsEmpty(MovementCarriersViewModel model)
        {
            Assert.Equal(string.Empty, model.FirstReg);
            Assert.Equal(string.Empty, model.FirstName);
            Assert.Equal(string.Empty, model.FirstTel);
            Assert.Equal(string.Empty, model.FirstFax);
            Assert.Equal(string.Empty, model.FirstEmail);
            Assert.Equal(string.Empty, model.FirstAddress);
        }

        private void AssertSecondCarrierIsEmpty(MovementCarriersViewModel model)
        {
            Assert.Equal(string.Empty, model.SecondReg);
            Assert.Equal(string.Empty, model.SecondName);
            Assert.Equal(string.Empty, model.SecondTel);
            Assert.Equal(string.Empty, model.SecondFax);
            Assert.Equal(string.Empty, model.SecondEmail);
            Assert.Equal(string.Empty, model.SecondAddress);
        }

        private void AssertLastCarrierIsEmpty(MovementCarriersViewModel model)
        {
            Assert.Equal(string.Empty, model.LastReg);
            Assert.Equal(string.Empty, model.LastName);
            Assert.Equal(string.Empty, model.LastTel);
            Assert.Equal(string.Empty, model.LastFax);
            Assert.Equal(string.Empty, model.LastEmail);
            Assert.Equal(string.Empty, model.LastAddress);
        }

        private void AssertFirstCarrierIsDefault(MovementCarriersViewModel model)
        {
            Assert.Equal(TestableBusiness.WasteSolutions.RegistrationNumber, model.FirstReg);
            Assert.Equal(TestableBusiness.WasteSolutions.Name, model.FirstName);
            Assert.Equal(TestableContact.BillyKnuckles.Telephone, model.FirstTel);
            Assert.Equal(string.Empty, model.FirstFax);
            Assert.Equal(TestableContact.BillyKnuckles.Email, model.FirstEmail);
        }

        private void AssertSecondCarrierIsDefault(MovementCarriersViewModel model)
        {
            Assert.Equal(TestableBusiness.WasteSolutions.RegistrationNumber, model.SecondReg);
            Assert.Equal(TestableBusiness.WasteSolutions.Name, model.SecondName);
            Assert.Equal(TestableContact.BillyKnuckles.Telephone, model.SecondTel);
            Assert.Equal(string.Empty, model.SecondFax);
            Assert.Equal(TestableContact.BillyKnuckles.Email, model.SecondEmail);
        }

        private void AssertLastCarrierIsDefault(MovementCarriersViewModel model)
        {
            Assert.Equal(TestableBusiness.WasteSolutions.RegistrationNumber, model.LastReg);
            Assert.Equal(TestableBusiness.WasteSolutions.Name, model.LastName);
            Assert.Equal(TestableContact.BillyKnuckles.Telephone, model.LastTel);
            Assert.Equal(string.Empty, model.LastFax);
            Assert.Equal(TestableContact.BillyKnuckles.Email, model.LastEmail);
        }

        private void AssertCarrierDetailsSet(MovementCarriersViewModel model)
        {
            Assert.Equal(4, model.CarrierDetails.Count);
        }
    }
}
