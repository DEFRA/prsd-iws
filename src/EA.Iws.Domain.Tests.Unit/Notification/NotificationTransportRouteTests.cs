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

        [Fact]
        public void AddStateOfImport_WithNullState_Throws()
        {
            var notification = GetTestNotification();
            Assert.Throws<ArgumentNullException>(() => notification.AddStateOfImportToNotification(null));
        }

        [Fact]
        public void AddStateOfImport_NotificationAlreadyHasStateOfImport_Throws()
        {
            var notification = GetTestNotification();

            var competentAuthority = ObjectInstantiator<CompetentAuthority>.CreateNew();
            var entryPoint = ObjectInstantiator<EntryOrExitPoint>.CreateNew();

            ObjectInstantiator<CompetentAuthority>.SetProperty(x => x.Country, ObjectInstantiator<Country>.CreateNew(), competentAuthority);
            ObjectInstantiator<EntryOrExitPoint>.SetProperty(x => x.Country, ObjectInstantiator<Country>.CreateNew(), entryPoint);

            var stateOfImport = new StateOfImport(ObjectInstantiator<Country>.CreateNew(),
                competentAuthority,
                entryPoint);

            notification.AddStateOfImportToNotification(stateOfImport);

            Assert.Throws<InvalidOperationException>(() => notification.AddStateOfImportToNotification(stateOfImport));
        }

        [Fact]
        public void AddStateOfImport_SameCountryToStateOfExport_Throws()
        {
            // Arrange
            var notification = GetTestNotification();
            
            var exportCountry = GetCountry(new Guid("053443D4-EFDC-4DC5-8772-D8E5DA52226C"));

            var exportCompetentAuthority = ObjectInstantiator<CompetentAuthority>.CreateNew();
            var exportExitPoint = ObjectInstantiator<EntryOrExitPoint>.CreateNew();
            
            ObjectInstantiator<CompetentAuthority>.SetProperty(x => x.Country, exportCountry, exportCompetentAuthority);
            ObjectInstantiator<EntryOrExitPoint>.SetProperty(x => x.Country, exportCountry, exportExitPoint);

            var importCompetentAuthority = ObjectInstantiator<CompetentAuthority>.CreateNew();
            var importExitPoint = ObjectInstantiator<EntryOrExitPoint>.CreateNew();

            ObjectInstantiator<CompetentAuthority>.SetProperty(x => x.Country, exportCountry, importCompetentAuthority);
            ObjectInstantiator<EntryOrExitPoint>.SetProperty(x => x.Country, exportCountry, importExitPoint);

            var stateOfExport = new StateOfExport(exportCountry,
                exportCompetentAuthority,
                exportExitPoint);
            var stateOfImport = new StateOfImport(exportCountry, importCompetentAuthority, importExitPoint);

            // Act
            notification.AddStateOfExportToNotification(stateOfExport);

            // Assert
            Assert.Throws<InvalidOperationException>(() => notification.AddStateOfImportToNotification(stateOfImport));
        }

        [Fact]
        public void AddStateOfImport_DifferentCountryToStateOfExport_Throws()
        {
            // Arrange
            var notification = GetTestNotification();
            var importCountryId = new Guid("98F1CEA6-5474-429C-AECC-45030C3B1463");

            var exportCountry = GetCountry(new Guid("053443D4-EFDC-4DC5-8772-D8E5DA52226C"));
            var importCountry = GetCountry(importCountryId);

            var exportCompetentAuthority = ObjectInstantiator<CompetentAuthority>.CreateNew();
            var exportExitPoint = ObjectInstantiator<EntryOrExitPoint>.CreateNew();

            ObjectInstantiator<CompetentAuthority>.SetProperty(x => x.Country, exportCountry, exportCompetentAuthority);
            ObjectInstantiator<EntryOrExitPoint>.SetProperty(x => x.Country, exportCountry, exportExitPoint);

            var importCompetentAuthority = ObjectInstantiator<CompetentAuthority>.CreateNew();
            var importExitPoint = ObjectInstantiator<EntryOrExitPoint>.CreateNew();

            ObjectInstantiator<CompetentAuthority>.SetProperty(x => x.Country, importCountry, importCompetentAuthority);
            ObjectInstantiator<EntryOrExitPoint>.SetProperty(x => x.Country, importCountry, importExitPoint);

            var stateOfExport = new StateOfExport(exportCountry,
                exportCompetentAuthority,
                exportExitPoint);
            var stateOfImport = new StateOfImport(importCountry, importCompetentAuthority, importExitPoint);

            // Act
            notification.AddStateOfExportToNotification(stateOfExport);
            notification.AddStateOfImportToNotification(stateOfImport);

            // Assert
            Assert.Equal(notification.StateOfImport.Country.Id, importCountryId);
        }

        private NotificationApplication GetTestNotification()
        {
            return new NotificationApplication(Guid.Empty, NotificationType.Disposal,
                UKCompetentAuthority.England, 650);
        }

        private Country GetCountry(Guid id)
        {
            var country = ObjectInstantiator<Country>.CreateNew();
            ObjectInstantiator<Country>.SetProperty(x => x.Id, id, country);
            return country;
        }
    }
}
