namespace EA.Iws.RequestHandlers.Tests.Unit.CustomsOffice
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Cqrs.Tests.Unit.Helpers;
    using DataAccess;
    using Domain;
    using Domain.Notification;
    using FakeItEasy;
    using RequestHandlers.CustomsOffice;
    using Requests.CustomsOffice;
    using TestHelpers.Helpers;
    using Xunit;
    using NotificationType = Domain.Notification.NotificationType;
    using StateOfExport = Domain.TransportRoute.StateOfExport;
    using StateOfImport = Domain.TransportRoute.StateOfImport;

    public class SetExitCustomsOfficeForNotificationByIdHandlerTests
    {
        private static readonly string AnyName = "name";
        private static readonly string AnyAddress = "address";
        private static readonly Guid AnyGuid = Guid.Empty;
        private readonly Country nonEuCountry;
        private readonly IwsContext context;
        private readonly DbContextHelper dbContextHelper;
        private readonly SetExitCustomsOfficeForNotificationByIdHandler handler;
        private readonly NotificationApplication anyNotification;
        private readonly Country country;
        private readonly StateOfExport stateOfExport;
        private readonly StateOfImport stateOfImportNonEu;

        public SetExitCustomsOfficeForNotificationByIdHandlerTests()
        {
            this.context = A.Fake<IwsContext>();
            this.dbContextHelper = new DbContextHelper();
            this.handler = new SetExitCustomsOfficeForNotificationByIdHandler(context);
           
            anyNotification = new NotificationApplication(AnyGuid, NotificationType.Recovery, UKCompetentAuthority.England, 0);
            
            country = CountryFactory.Create(AnyGuid);
            
            nonEuCountry = CountryFactory.Create(new Guid("606ECF5A-6571-4803-9CCA-7E1AF82D147A"), "test", false);

            A.CallTo(() => context.Countries).Returns(dbContextHelper.GetAsyncEnabledDbSet(new[]
            {
                country,
                nonEuCountry
            }));

            stateOfExport = new StateOfExport(country, 
                CompetentAuthorityFactory.Create(AnyGuid, country), 
                EntryOrExitPointFactory.Create(AnyGuid, country));

            stateOfImportNonEu = new StateOfImport(nonEuCountry,
                CompetentAuthorityFactory.Create(new Guid("5E4F1F22-5054-449B-9EC7-933A43BB5D6D"), nonEuCountry),
                EntryOrExitPointFactory.Create(new Guid("9781324F-17B8-4009-89B2-C18963E3E6E1"), nonEuCountry));
        }

        [Fact]
        public async Task NotificationDoesNotExist_Throws()
        {
            A.CallTo(() => context.NotificationApplications)
                .Returns(dbContextHelper.GetAsyncEnabledDbSet(new NotificationApplication[] { }));

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(new SetExitCustomsOfficeForNotificationById(AnyGuid, AnyName, AnyAddress, AnyGuid)));
        }

        [Fact]
        public async Task NotificationExistsButDoesNotRequireCustomsOffice_Throws()
        {
            var dbSet = dbContextHelper.GetAsyncEnabledDbSet(new[] { anyNotification });
            A.CallTo(() => context.NotificationApplications).Returns(dbSet);

            var request = new SetExitCustomsOfficeForNotificationById(anyNotification.Id,
                AnyName, 
                AnyAddress, 
                country.Id);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(request));
        }

        [Fact]
        public async Task NotificationExistsAndRequiresExitCustomsOffice_SetsExitOffice()
        {
            anyNotification.AddStateOfExportToNotification(stateOfExport);
            anyNotification.SetStateOfImportForNotification(stateOfImportNonEu);

            var notifications = dbContextHelper.GetAsyncEnabledDbSet(new[] { anyNotification });
            A.CallTo(() => context.NotificationApplications).Returns(notifications);

            var request = new SetExitCustomsOfficeForNotificationById(AnyGuid,
                AnyName,
                AnyAddress,
                country.Id);

            await handler.HandleAsync(request);

            var notification = context.NotificationApplications.First();

            Assert.Equal(request.Name, notification.ExitCustomsOffice.Name);
        }
    }
}
