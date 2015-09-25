namespace EA.Iws.RequestHandlers.Tests.Unit.CustomsOffice
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.TransportRoute;
    using FakeItEasy;
    using RequestHandlers.CustomsOffice;
    using Requests.CustomsOffice;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetExitCustomsOfficeForNotificationByIdHandlerTests
    {
        private static readonly string AnyName = "name";
        private static readonly string AnyAddress = "address";
        private static readonly Guid AnyGuid = Guid.Empty;
        private readonly Country nonEuCountry;
        private readonly IwsContext context;
        private readonly SetExitCustomsOfficeForNotificationByIdHandler handler;
        private readonly NotificationApplication anyNotification;
        private readonly Guid notificationId = new Guid("467CE8AC-71E2-4E6B-8162-FABC997D94FC");
        private readonly Country country;
        private readonly StateOfExport stateOfExport;
        private readonly StateOfImport stateOfImportNonEu;
        private readonly TransportRoute transport;

        public SetExitCustomsOfficeForNotificationByIdHandlerTests()
        {
            this.context = new TestIwsContext();
            var repository = A.Fake<ITransportRouteRepository>();
           
            anyNotification = new NotificationApplication(TestIwsContext.UserId, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            EntityHelper.SetEntityId(anyNotification, notificationId);

            transport = new TransportRoute(notificationId);

            context.NotificationApplications.Add(anyNotification);
            context.TransportRoutes.Add(transport);

            country = CountryFactory.Create(AnyGuid);
            nonEuCountry = CountryFactory.Create(new Guid("606ECF5A-6571-4803-9CCA-7E1AF82D147A"), "test", false);

            context.Countries.AddRange(new[]
            {
                country,
                nonEuCountry
            });

            stateOfExport = new StateOfExport(country, 
                CompetentAuthorityFactory.Create(AnyGuid, country), 
                EntryOrExitPointFactory.Create(AnyGuid, country));

            stateOfImportNonEu = new StateOfImport(nonEuCountry,
                CompetentAuthorityFactory.Create(new Guid("5E4F1F22-5054-449B-9EC7-933A43BB5D6D"), nonEuCountry),
                EntryOrExitPointFactory.Create(new Guid("9781324F-17B8-4009-89B2-C18963E3E6E1"), nonEuCountry));

            A.CallTo(() => repository.GetByNotificationId(notificationId)).Returns(transport);

            this.handler = new SetExitCustomsOfficeForNotificationByIdHandler(context, repository);
        }

        [Fact]
        public async Task NotificationExistsButDoesNotRequireCustomsOffice_Throws()
        {
            var request = new SetExitCustomsOfficeForNotificationById(notificationId,
                AnyName, 
                AnyAddress, 
                country.Id);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(request));
        }

        [Fact]
        public async Task NotificationExistsAndRequiresExitCustomsOffice_SetsExitOffice()
        {
            transport.SetStateOfExportForNotification(stateOfExport);
            transport.SetStateOfImportForNotification(stateOfImportNonEu);

            var request = new SetExitCustomsOfficeForNotificationById(notificationId,
                AnyName,
                AnyAddress,
                country.Id);

            await handler.HandleAsync(request);

            var transportRoute = context.TransportRoutes.First();

            Assert.Equal(request.Name, transportRoute.ExitCustomsOffice.Name);
        }
    }
}
