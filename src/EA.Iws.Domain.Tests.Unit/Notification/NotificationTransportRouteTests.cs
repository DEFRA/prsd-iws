namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using Domain.Notification;
    using Helpers;
    using TransportRoute;
    using Xunit;

    public class NotificationTransportRouteTests
    {
        [Fact]
        public void AddStateOfExport_WithNullState_Throws()
        {
            var notification = GetTestNotification();
            Assert.Throws<ArgumentNullException>(() => notification.AddStateOfExportToNotification(null));
        }

        [Fact]
        public void AddStateOfExport_NotificationAlreadyHasStateOfExport_Throws()
        {
            var notification = GetTestNotification();

            var competentAuthority = ObjectInstantiator<CompetentAuthority>.CreateNew();
            var exitPoint = ObjectInstantiator<EntryOrExitPoint>.CreateNew();

            ObjectInstantiator<CompetentAuthority>.SetProperty(x => x.Country, ObjectInstantiator<Country>.CreateNew(), competentAuthority);
            ObjectInstantiator<EntryOrExitPoint>.SetProperty(x => x.Country, ObjectInstantiator<Country>.CreateNew(), exitPoint);

            var stateOfExport = new StateOfExport(ObjectInstantiator<Country>.CreateNew(), 
                competentAuthority, 
                exitPoint);

            notification.AddStateOfExportToNotification(stateOfExport);

            Assert.Throws<InvalidOperationException>(() => notification.AddStateOfExportToNotification(stateOfExport));
        }

        private NotificationApplication GetTestNotification()
        {
            return new NotificationApplication(Guid.Empty, NotificationType.Disposal,
                UKCompetentAuthority.England, 650);
        }
    }
}
