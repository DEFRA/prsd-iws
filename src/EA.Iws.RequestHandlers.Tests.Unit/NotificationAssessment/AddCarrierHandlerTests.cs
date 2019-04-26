namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using RequestHandlers.NotificationAssessment;
    using Requests.NotificationAssessment;
    using TestHelpers.Helpers;
    using Xunit;

    public class AddCarrierHandlerTests
    {
        private readonly AddCarrierHandler handler;
        private readonly ICountryRepository countryRepository;
        private readonly ICarrierRepository repository;

        private readonly Guid notificationId;
        private readonly Guid countryId;
        private readonly Guid carrierId;
        private const string AnyString = "test";

        public AddCarrierHandlerTests()
        {
            notificationId = Guid.NewGuid();
            countryId = Guid.NewGuid();
            carrierId = new Guid();

            countryRepository = A.Fake<ICountryRepository>();
            repository = A.Fake<ICarrierRepository>();

            var context = new TestIwsContext();
            context.Users.Add(UserFactory.Create(TestIwsContext.UserId, AnyString, AnyString, AnyString, AnyString));

            var carrierCollection = new CarrierCollection(notificationId);

            A.CallTo(() => repository.GetByNotificationId(notificationId)).Returns(carrierCollection);

            handler = new AddCarrierHandler(context, repository, countryRepository);
        }

        [Fact]
        public async Task AddCarrierHandler_GetsCarriers()
        {
            var request = GetRequest();

            await handler.HandleAsync(request);

            A.CallTo(() => repository.GetByNotificationId(notificationId)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task AddCarrierHandler_GetsCountryById()
        {
            var request = GetRequest();

            await handler.HandleAsync(request);

            A.CallTo(() => countryRepository.GetById(countryId)).MustHaveHappened(Repeated.Exactly.Once);
        }

        private AddCarrier GetRequest()
        {
            return new AddCarrier()
            {
                NotificationId = notificationId,
                Business = new BusinessInfoData()
                {
                    Name = AnyString,
                    BusinessType = BusinessType.SoleTrader,
                    RegistrationNumber = AnyString,
                    AdditionalRegistrationNumber = AnyString,
                    OtherDescription = AnyString
                },
                Address = new AddressData
                {
                    Address2 = AnyString,
                    CountryId = countryId,
                    CountryName = AnyString,
                    PostalCode = AnyString,
                    Region = AnyString,
                    StreetOrSuburb = AnyString,
                    TownOrCity = AnyString
                },

                Contact = new ContactData
                {
                    Email = AnyString,
                    FullName = AnyString,
                    Telephone = AnyString
                }
            };
        }
    }
}
