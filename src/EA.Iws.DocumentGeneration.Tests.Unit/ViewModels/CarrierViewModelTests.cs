namespace EA.Iws.DocumentGeneration.Tests.Unit.ViewModels
{
    using System.Collections.Generic;
    using DocumentGeneration.Formatters;
    using DocumentGeneration.ViewModels;
    using Domain.NotificationApplication;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class CarrierViewModelTests
    {
        private const string AnyString = "test";

        private readonly MeansOfTransportFormatter formatter = new MeansOfTransportFormatter();
        private readonly TestableNotificationApplication notificiation;
        private readonly TestableCarrier firstCarrier;
        private readonly TestableCarrier secondCarrier;
        private readonly List<Carrier> carriers = new List<Carrier>(); 

        public CarrierViewModelTests()
        {
            firstCarrier = new TestableCarrier
            {
                Address = TestableAddress.AddlestoneAddress,
                Business = TestableBusiness.WasteSolutions,
                Contact = TestableContact.BillyKnuckles
            };
            
            secondCarrier = new TestableCarrier
            {
                Address = TestableAddress.WitneyAddress,
                Business = TestableBusiness.CSharpGarbageCollector,
                Contact = TestableContact.MikeMerry
            };

            notificiation = new TestableNotificationApplication
            {
                Carriers = carriers
            };
        }

        [Fact]
        public void NotificationIsNull_ReturnsEmptyList()
        {
            var result = CarrierViewModel.CreateCarrierViewModelsForNotification(null, formatter);

            Assert.Empty(result);
        }

        [Fact]
        public void NotificationHasNoCarriers_ReturnsEmptyList()
        {
            var result = CarrierViewModel.CreateCarrierViewModelsForNotification(notificiation, formatter);

            Assert.Empty(result);
        }

        [Fact]
        public void NotificationHasOneCarrier_ReturnsListWithOneItem()
        {
            carriers.Add(firstCarrier);

            var result = CarrierViewModel.CreateCarrierViewModelsForNotification(notificiation, formatter);

            Assert.Equal(1, result.Count);
        }

        [Fact]
        public void CarrierIsNull_ReturnsModelWithEmptyFields()
        {
            var result = new CarrierViewModel(null, string.Empty);

            Assert.Equal(string.Empty, result.ContactPerson);
            Assert.Equal(string.Empty, result.Email);
            Assert.Equal(string.Empty, result.Fax);
            Assert.Equal(string.Empty, result.RegistrationNumber);
            Assert.Equal(string.Empty, result.Telephone);
            Assert.Equal(string.Empty, result.Name);
            Assert.Equal(string.Empty, result.AnnexMessage);
            Assert.Equal(string.Empty, result.Address);
        }

        [Fact]
        public void MeansOfTransportIsNull_ReturnsMeansOfTransportAsEmptyString()
        {
            var result = new CarrierViewModel(firstCarrier, null);

            Assert.Equal(string.Empty, result.MeansOfTransport);
        }
        
        [Fact]
        public void SetsMeansOfTransport()
        {
            var meansOfTransport = "Road Sea Road";

            var result = new CarrierViewModel(firstCarrier, meansOfTransport);

            Assert.Equal(meansOfTransport, result.MeansOfTransport);
        }

        [Fact]
        public void SetsContactName()
        {
            var result = new CarrierViewModel(firstCarrier, string.Empty);

            Assert.Equal(string.Format("{0} {1}", firstCarrier.Contact.FirstName, firstCarrier.Contact.LastName), 
                result.ContactPerson);
        }

        [Fact]
        public void SetsFax()
        {
            firstCarrier.Contact = new TestableContact
            {
                Email = AnyString,
                Fax = "01353450",
                FirstName = AnyString,
                LastName = AnyString,
                Telephone = AnyString
            };

            var result = new CarrierViewModel(firstCarrier, string.Empty);

            Assert.Equal(firstCarrier.Contact.Fax,
                result.Fax);
        }

        [Fact]
        public void SetsTelephone()
        {
            var result = new CarrierViewModel(firstCarrier, string.Empty);

            Assert.Equal(firstCarrier.Contact.Telephone,
                result.Telephone);
        }

        [Fact]
        public void SetsEmail()
        {
            var result = new CarrierViewModel(firstCarrier, string.Empty);

            Assert.Equal(firstCarrier.Contact.Email,
                result.Email);
        }

        [Fact]
        public void SetsName()
        {
            var result = new CarrierViewModel(firstCarrier, string.Empty);

            Assert.Equal(firstCarrier.Business.Name,
                result.Name);
        }

        [Fact]
        public void SetsRegistrationNumber()
        {
            var result = new CarrierViewModel(firstCarrier, string.Empty);

            Assert.Equal(firstCarrier.Business.RegistrationNumber,
                result.RegistrationNumber);
        }
    }
}
