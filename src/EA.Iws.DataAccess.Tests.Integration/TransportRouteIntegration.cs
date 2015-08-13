namespace EA.Iws.DataAccess.Tests.Integration
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Domain.NotificationApplication;
    using Domain.TransportRoute;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using Xunit;

    [Trait("Category", "Integration")]
    public class TransportRouteIntegration
    {
        private readonly IwsContext context;

        public TransportRouteIntegration()
        {
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => userContext.UserId).Returns(Guid.NewGuid());

            context = new IwsContext(userContext, A.Fake<IEventDispatcher>());
        }

        [Fact]
        public async Task CanAddStateOfExport()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal, UKCompetentAuthority.England, 450);

            context.NotificationApplications.Add(notification);

            await context.SaveChangesAsync();

            var exitPoint = await context.EntryOrExitPoints.FirstAsync();

            var country = exitPoint.Country;

            var competentAuthority = await context.CompetentAuthorities.FirstAsync(ca => ca.Country.Id == country.Id);

            var stateOfExport = new StateOfExport(country, competentAuthority, exitPoint);

            notification.SetStateOfExportForNotification(stateOfExport);

            await context.SaveChangesAsync();

            Assert.Equal(country.Id, notification.StateOfExport.Country.Id);
            Assert.Equal(competentAuthority.Id, notification.StateOfExport.CompetentAuthority.Id);
            Assert.Equal(exitPoint.Id, notification.StateOfExport.ExitPoint.Id);

            context.DeleteOnCommit(stateOfExport);
            context.DeleteOnCommit(notification);
            await context.SaveChangesAsync();
        }

        [Fact]
        public void CanUpdateStateOfExport()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Disposal, UKCompetentAuthority.England, 450);

            context.NotificationApplications.Add(notification);

            context.SaveChanges();

            var exitPoint = context.EntryOrExitPoints.First();

            var country = exitPoint.Country;

            var competentAuthority = context.CompetentAuthorities.First(ca => ca.Country.Id == country.Id);

            var stateOfExport = new StateOfExport(country, competentAuthority, exitPoint);

            notification.SetStateOfExportForNotification(stateOfExport);

            context.SaveChanges();

            var nextExitPoint = context.EntryOrExitPoints.First(ep => ep.Id != exitPoint.Id);

            if (nextExitPoint.Country.Id != country.Id)
            {
                country = nextExitPoint.Country;
                competentAuthority = context.CompetentAuthorities.First(ca => ca.Country.Id == country.Id);
            }

            var newStateOfExport = new StateOfExport(country, competentAuthority, nextExitPoint);
            notification.SetStateOfExportForNotification(newStateOfExport);

            context.SaveChanges();

            Assert.Equal(nextExitPoint.Id, notification.StateOfExport.ExitPoint.Id);

            context.DeleteOnCommit(notification);

            context.SaveChanges();
        }
    }
}
