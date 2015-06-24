namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.Notification;
    using TestHelpers.Helpers;
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

            var competentAuthority = GetTestCompetentAuthority(GetTestCountry(Guid.Empty));
            var exitPoint = GetTestEntryOrExitPoint(GetTestCountry(Guid.Empty));

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
            Assert.Throws<ArgumentNullException>(() => notification.SetStateOfImportForNotification(null));
        }

        [Fact]
        public void SetStateOfImport_NotificationAlreadyHasStateOfImport_ReplacesStateOfImport()
        {
            var notification = GetTestNotification();

            var competentAuthority = GetTestCompetentAuthority(GetTestCountry(Guid.Empty));
            var entryPoint = GetTestEntryOrExitPoint(GetTestCountry(Guid.Empty));

            var stateOfImport = new StateOfImport(ObjectInstantiator<Country>.CreateNew(),
                competentAuthority,
                entryPoint);

            notification.SetStateOfImportForNotification(stateOfImport);

            notification.SetStateOfImportForNotification(stateOfImport);

            Assert.Equal(stateOfImport.CompetentAuthority.Id, notification.StateOfImport.CompetentAuthority.Id);
        }

        [Fact]
        public void SetStateOfImport_SameCountryToStateOfExport_Throws()
        {
            // Arrange
            var notification = GetTestNotification();

            var exportCountry = GetTestCountry(new Guid("053443D4-EFDC-4DC5-8772-D8E5DA52226C"));

            var exportCompetentAuthority = GetTestCompetentAuthority(exportCountry);
            var exportExitPoint = GetTestEntryOrExitPoint(exportCountry);

            var importCompetentAuthority = GetTestCompetentAuthority(exportCountry);
            var importExitPoint = GetTestEntryOrExitPoint(exportCountry);

            var stateOfExport = new StateOfExport(exportCountry,
                exportCompetentAuthority,
                exportExitPoint);

            var stateOfImport = new StateOfImport(exportCountry,
                importCompetentAuthority,
                importExitPoint);

            // Act
            notification.AddStateOfExportToNotification(stateOfExport);

            // Assert
            Assert.Throws<InvalidOperationException>(() => notification.SetStateOfImportForNotification(stateOfImport));
        }

        [Fact]
        public void SetStateOfImport_DifferentCountryToStateOfExport_Throws()
        {
            // Arrange
            var notification = GetTestNotification();
            var importCountryId = new Guid("98F1CEA6-5474-429C-AECC-45030C3B1463");

            var exportCountry = GetTestCountry(new Guid("053443D4-EFDC-4DC5-8772-D8E5DA52226C"));
            var importCountry = GetTestCountry(importCountryId);

            var exportCompetentAuthority = GetTestCompetentAuthority(exportCountry);
            var exportExitPoint = GetTestEntryOrExitPoint(exportCountry);

            var importCompetentAuthority = GetTestCompetentAuthority(importCountry);
            var importExitPoint = GetTestEntryOrExitPoint(importCountry);

            var stateOfExport = new StateOfExport(exportCountry,
                exportCompetentAuthority,
                exportExitPoint);

            var stateOfImport = new StateOfImport(importCountry,
                importCompetentAuthority,
                importExitPoint);

            // Act
            notification.AddStateOfExportToNotification(stateOfExport);
            notification.SetStateOfImportForNotification(stateOfImport);

            // Assert
            Assert.Equal(notification.StateOfImport.Country.Id, importCountryId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void TransitState_OrdinalPositionZeroOrLess_Throws(int position)
        {
            var country = GetTestCountry(new Guid("C4E8BFE2-473D-42CC-8AC1-5C499699B925"));

            Assert.Throws<ArgumentOutOfRangeException>(() => new TransitState(country,
                GetTestCompetentAuthority(country),
                GetTestEntryOrExitPoint(country, new Guid("2D601D3B-00BA-4AE9-9006-D6F7FBDC6898")),
                GetTestEntryOrExitPoint(country, Guid.Empty),
                position));
        }

        [Fact]
        public void TransitState_EntryAndExitPointTheSame_Throws()
        {
            var country = GetTestCountry(Guid.Empty);

            Assert.Throws<InvalidOperationException>(() => new TransitState(country,
                GetTestCompetentAuthority(country),
                GetTestEntryOrExitPoint(country, new Guid("E0B04105-8E22-49E6-A00C-CBD2F2D11B54")),
                GetTestEntryOrExitPoint(country, new Guid("E0B04105-8E22-49E6-A00C-CBD2F2D11B54")),
                1));
        }

        [Theory]
        [InlineData("50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "AAC022B3-ECE8-4BFE-9D69-216B3F4C13D0")]
        [InlineData("50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "AAC022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0")]
        [InlineData("50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "AAC022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0")]
        [InlineData("AAC022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0")]
        [InlineData("AAC022B3-ECE8-4BFE-9D69-216B3F4C13D0", "AAC022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0")]
        public void TransitState_ParametersNotInTheSameCountry_Throws(string countryId, string competentAuthorityId, string entryPointId, string exitPointId)
        {
            var country = GetTestCountry(new Guid(countryId));
            var competentAuthority = GetTestCompetentAuthority(GetTestCountry(new Guid(competentAuthorityId)));
            var entryPoint = GetTestEntryOrExitPoint(GetTestCountry(new Guid(entryPointId)));
            var exitPoint = GetTestEntryOrExitPoint(GetTestCountry(new Guid(exitPointId)));

            Assert.Throws<InvalidOperationException>(
                () => new TransitState(country, competentAuthority, entryPoint, exitPoint, 1));
        }

        [Fact]
        public void AddTransitState_OnlyTransitState_AddedSuccessfully()
        {
            var notification = GetTestNotification();
            var country = GetTestCountry(new Guid("836BEFCC-A2DA-454C-B5B9-DD72AFDAC543"));

            var transitState = new TransitState(country,
                GetTestCompetentAuthority(country),
                GetTestEntryOrExitPoint(country, Guid.Empty),
                GetTestEntryOrExitPoint(country, new Guid("E8E2D79A-F1CA-4928-9ED8-AAF961E2B7B7")),
                1);

            notification.AddTransitStateToNotification(transitState);

            Assert.True(notification.TransitStates.Count() == 1);
        }

        [Fact]
        public void AddTransitState_ToAlreadyOccupiedPosition_Throws()
        {
            var guids = GetGuids();

            var notification = GetTestNotification();
            var firstCountry = GetTestCountry(guids[0]);

            var firstTransitState = new TransitState(firstCountry, 
                GetTestCompetentAuthority(firstCountry),
                GetTestEntryOrExitPoint(firstCountry, guids[1]),
                GetTestEntryOrExitPoint(firstCountry, guids[2]), 
                1);

            notification.AddTransitStateToNotification(firstTransitState);

            var secondCountry = GetTestCountry(guids[1]);
            var secondTransitState = new TransitState(secondCountry,
                GetTestCompetentAuthority(secondCountry),
                GetTestEntryOrExitPoint(secondCountry, guids[3]),
                GetTestEntryOrExitPoint(secondCountry, guids[4]),
                1);

            Assert.Throws<InvalidOperationException>(
                () => notification.AddTransitStateToNotification(secondTransitState));
        }

        [Fact]
        public void AddTransitState_ToPositionOutOfRange_Throws()
        {
            var guids = GetGuids();

            var notification = GetTestNotification();
            var firstCountry = GetTestCountry(guids[0]);

            var firstTransitState = new TransitState(firstCountry,
                GetTestCompetentAuthority(firstCountry),
                GetTestEntryOrExitPoint(firstCountry, guids[1]),
                GetTestEntryOrExitPoint(firstCountry, guids[2]),
                1);

            notification.AddTransitStateToNotification(firstTransitState);

            var thirdCountry = GetTestCountry(guids[1]);
            var thirdTransitState = new TransitState(thirdCountry,
                GetTestCompetentAuthority(thirdCountry),
                GetTestEntryOrExitPoint(thirdCountry, guids[3]),
                GetTestEntryOrExitPoint(thirdCountry, guids[4]),
                3);

            Assert.Throws<InvalidOperationException>(() => notification.AddTransitStateToNotification(thirdTransitState));
        }

        [Fact]
        public void AddTransitState_ToAvailablePosition_AddsToNotification()
        {
            var guids = GetGuids();

            var notification = GetTestNotification();
            var firstCountry = GetTestCountry(guids[0]);
            var secondCountry = GetTestCountry(guids[1]);

            var firstTransitState = new TransitState(firstCountry,
                GetTestCompetentAuthority(firstCountry),
                GetTestEntryOrExitPoint(firstCountry, guids[1]),
                GetTestEntryOrExitPoint(firstCountry, guids[2]),
                1);

            notification.AddTransitStateToNotification(firstTransitState);

            var secondTransitState = new TransitState(secondCountry,
                GetTestCompetentAuthority(secondCountry),
                GetTestEntryOrExitPoint(secondCountry, guids[3]),
                GetTestEntryOrExitPoint(secondCountry, guids[4]),
                2);

            notification.AddTransitStateToNotification(secondTransitState);

            Assert.Equal(2, notification.TransitStates.Count());
        }

        [Fact]
        public void AddTransitState_ToThirdPosition_AddsToNotification()
        {
            var guids = GetGuids();

            var notification = GetTestNotification();
            var firstCountry = GetTestCountry(guids[0]);
            var secondCountry = GetTestCountry(guids[1]);
            var thirdCountry = GetTestCountry(guids[2]);

            var firstTransitState = new TransitState(firstCountry,
                GetTestCompetentAuthority(firstCountry),
                GetTestEntryOrExitPoint(firstCountry, guids[1]),
                GetTestEntryOrExitPoint(firstCountry, guids[2]),
                1);

            notification.AddTransitStateToNotification(firstTransitState);

            var secondTransitState = new TransitState(secondCountry,
                GetTestCompetentAuthority(secondCountry),
                GetTestEntryOrExitPoint(secondCountry, guids[3]),
                GetTestEntryOrExitPoint(secondCountry, guids[4]),
                2);

            notification.AddTransitStateToNotification(secondTransitState);

            var thirdTransitState = new TransitState(thirdCountry,
                GetTestCompetentAuthority(thirdCountry),
                GetTestEntryOrExitPoint(thirdCountry, guids[5]),
                GetTestEntryOrExitPoint(thirdCountry, guids[6]),
                3);

            notification.AddTransitStateToNotification(thirdTransitState);

            Assert.Equal(3, notification.TransitStates.Count());
        }

        [Fact]
        public void GetAvailableTransitStatePositions_WhereNotificationIsEmpty_ReturnsOne()
        {
            var notification = GetTestNotification();

            int[] result = notification.GetAvailableTransitStatePositions();

            Assert.Equal(1, result[0]);
        }

        [Fact]
        public void GetAvailableTransitStatePositions_WhereNotificationHasOneState_ReturnsTwo()
        {
            var guids = GetGuids();

            var notification = GetTestNotification();
            var firstCountry = GetTestCountry(guids[0]);

            var firstTransitState = new TransitState(firstCountry,
                GetTestCompetentAuthority(firstCountry),
                GetTestEntryOrExitPoint(firstCountry, guids[1]),
                GetTestEntryOrExitPoint(firstCountry, guids[2]),
                1);

            notification.AddTransitStateToNotification(firstTransitState);

            int[] result = notification.GetAvailableTransitStatePositions();

            Assert.Equal(2, result[0]);
        }

        [Fact]
        public void GetAvailableTransitStatePositions_WhereNotificationHasTwoStates_ReturnsThree()
        {
            var guids = GetGuids();

            var notification = GetTestNotification();
            var firstCountry = GetTestCountry(guids[0]);
            var secondCountry = GetTestCountry(guids[1]);

            var firstTransitState = new TransitState(firstCountry,
                GetTestCompetentAuthority(firstCountry),
                GetTestEntryOrExitPoint(firstCountry, guids[1]),
                GetTestEntryOrExitPoint(firstCountry, guids[2]),
                1);

            notification.AddTransitStateToNotification(firstTransitState);

            var secondTransitState = new TransitState(secondCountry,
                GetTestCompetentAuthority(secondCountry),
                GetTestEntryOrExitPoint(secondCountry, guids[3]),
                GetTestEntryOrExitPoint(secondCountry, guids[4]),
                2);

            notification.AddTransitStateToNotification(secondTransitState);

            int[] result = notification.GetAvailableTransitStatePositions();

            Assert.Equal(3, result[0]);
        }

        private IList<Guid> GetGuids()
        {
            return new[]
            {
                new Guid("D8DB53F9-D3CB-498D-B044-7A67B94EE4E4"),
                new Guid("1D3A7A18-F345-4201-81AA-8FC8A4029F4E"),
                new Guid("596E6BE3-D0C1-4C17-AF75-BFCDBD972627"),
                new Guid("208EA94D-638B-4724-9272-DF1383FF9452"),
                new Guid("DEDAA6F6-CA6D-486E-B0B8-254A477E697F"),
                new Guid("4C7EEA30-510E-45BD-8DBB-3EC1AF5E563A"),
                new Guid("8B87B87D-43D0-4BBE-8E2C-8EC84C983A42"),
                new Guid("40E44905-4CDE-4470-8E8B-527EE65C65DC"),
                new Guid("58E18F1C-5A4A-4E2E-869F-72D9C1512FE8") 
            };
        }

        private NotificationApplication GetTestNotification()
        {
            return new NotificationApplication(Guid.Empty, NotificationType.Disposal,
                UKCompetentAuthority.England, 650);
        }

        private EntryOrExitPoint GetTestEntryOrExitPoint(Country country, Guid? id = null)
        {
            var entryOrExitPoint = ObjectInstantiator<EntryOrExitPoint>.CreateNew();
            ObjectInstantiator<EntryOrExitPoint>.SetProperty(x => x.Country, country, entryOrExitPoint);

            if (id != null)
            {
                ObjectInstantiator<EntryOrExitPoint>.SetProperty(x => x.Id, (Guid)id, entryOrExitPoint);
            }

            return entryOrExitPoint;
        }

        private Country GetTestCountry(Guid id)
        {
            var country = ObjectInstantiator<Country>.CreateNew();
            ObjectInstantiator<Country>.SetProperty(x => x.Id, id, country);
            return country;
        }

        private CompetentAuthority GetTestCompetentAuthority(Country country)
        {
            var competentAuthority = ObjectInstantiator<CompetentAuthority>.CreateNew();
            ObjectInstantiator<CompetentAuthority>.SetProperty(x => x.Country, country, competentAuthority);
            return competentAuthority;
        }
    }
}
