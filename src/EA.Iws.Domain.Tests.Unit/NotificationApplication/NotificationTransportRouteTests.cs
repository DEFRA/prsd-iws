﻿namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.TransportRoute;
    using FakeItEasy;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationTransportRouteTests
    {
        private readonly IList<Guid> guids;

        private readonly TransportRoute transportRoute;
        private readonly IList<Country> countries;
        private readonly IList<TransitState> transitStates;

        private readonly ITransportRouteValidator validator;

        public NotificationTransportRouteTests()
        {
            this.transportRoute = new TransportRoute(Guid.Empty);

            guids = new List<Guid>
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

            countries = guids.Select(g => CountryFactory.Create(g)).ToList();

            transitStates = new List<TransitState>();
            for (int i = 0; i < countries.Count - 1; i++)
            {
                var c = countries[i];
                transitStates.Add(new TransitState(c,
                CompetentAuthorityFactory.Create(guids[i], c),
                EntryOrExitPointFactory.Create(guids[i], c),
                EntryOrExitPointFactory.Create(guids[i + 1], c), i + 1));
                ObjectInstantiator<TransitState>.SetProperty(x => x.Id, guids[i], transitStates[i]);
            }

            this.validator = A.Fake<ITransportRouteValidator>();
        }

        [Fact]
        public void SetStateOfExport_WithNullStateAndNullCountries_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => transportRoute.SetStateOfExportForNotification(null, this.validator));
        }

        [Fact]
        public void SetStateOfExport_WithNullStateAndNonNullCountries_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => transportRoute.SetStateOfExportForNotification(null, this.validator));
        }

        [Fact]
        public void SetStateOfExport_WithNonNullStateAndNullCountries_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => transportRoute.SetStateOfExportForNotification(new TestableStateOfExport(), null));
        }

        [Fact]
        public void SetStateOfExport_NotificationAlreadyHasStateOfExport_Overwrites()
        {
            var competentAuthority = GetTestCompetentAuthority(countries[0]);
            var exitPoint = GetTestEntryOrExitPoint(countries[0]);

            var stateOfExport = new StateOfExport(countries[0],
                competentAuthority,
                exitPoint);

            var intraCountryExportAlloweds = new TestableIntraCountryExportAllowed[0];

            A.CallTo(() => this.validator.IsImportAndExportStatesCombinationValid(null, stateOfExport)).Returns(true);

            transportRoute.SetStateOfExportForNotification(stateOfExport, this.validator);

            Assert.Equal(stateOfExport.Country.Id, transportRoute.StateOfExport.Country.Id);
        }

        [Fact]
        public void SetStateOfImport_WithNullStateAndNullCountries_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => transportRoute.SetStateOfImportForNotification(null, null));
        }

        [Fact]
        public void SetStateOfImport_WithNullStateAndNonNullCountries_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => transportRoute.SetStateOfImportForNotification(null, this.validator));
        }

        [Fact]
        public void SetStateOfImport_WithNonNullStateAndNullCountries_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => transportRoute.SetStateOfImportForNotification(new TestableStateOfImport(), null));
        }

        [Fact]
        public void SetStateOfImport_NotificationAlreadyHasStateOfImport_ReplacesStateOfImport()
        {
            var competentAuthority = GetTestCompetentAuthority(countries[0]);
            var entryPoint = GetTestEntryOrExitPoint(countries[0]);

            var stateOfImport = new StateOfImport(countries[0],
                competentAuthority,
                entryPoint);

            var intraCountryExportAlloweds = new TestableIntraCountryExportAllowed[0];

            A.CallTo(() => this.validator.IsImportAndExportStatesCombinationValid(stateOfImport, null)).Returns(true);

            transportRoute.SetStateOfImportForNotification(stateOfImport, this.validator);

            transportRoute.SetStateOfImportForNotification(stateOfImport, this.validator);

            Assert.Equal(stateOfImport.CompetentAuthority.Id, transportRoute.StateOfImport.CompetentAuthority.Id);
        }

        [Fact]
        public void SetStateOfImport_SameCountryToStateOfExportIfNoSameCountryExportsAllowed_Throws()
        {
            // Arrange
            var exportCountry = countries[0];

            var exportCompetentAuthority = CompetentAuthorityFactory.Create(guids[0], exportCountry);
            var exportExitPoint = EntryOrExitPointFactory.Create(guids[0], exportCountry);

            var importCompetentAuthority = CompetentAuthorityFactory.Create(guids[1], exportCountry);
            var importExitPoint = EntryOrExitPointFactory.Create(guids[1], exportCountry);

            var stateOfExport = new StateOfExport(exportCountry,
                exportCompetentAuthority,
                exportExitPoint);

            var stateOfImport = new StateOfImport(exportCountry,
                importCompetentAuthority,
                importExitPoint);

            A.CallTo(() => this.validator.IsImportAndExportStatesCombinationValid(null, stateOfExport)).Returns(true);
            A.CallTo(() => this.validator.IsImportAndExportStatesCombinationValid(stateOfImport, stateOfExport)).Returns(false);

            // Act
            var intraCountryExportAlloweds = new TestableIntraCountryExportAllowed[0];
            transportRoute.SetStateOfExportForNotification(stateOfExport, this.validator);

            // Assert
            Assert.Throws<InvalidOperationException>(() => transportRoute.SetStateOfImportForNotification(stateOfImport, this.validator));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void TransitState_OrdinalPositionZeroOrLess_Throws(int position)
        {
            var country = countries[0];

            Assert.Throws<ArgumentOutOfRangeException>(() => new TransitState(country,
                GetTestCompetentAuthority(country),
                GetTestEntryOrExitPoint(country, new Guid("2D601D3B-00BA-4AE9-9006-D6F7FBDC6898")),
                GetTestEntryOrExitPoint(country, Guid.Empty),
                position));
        }

        [Fact]
        public void TransitState_EntryAndExitPointTheSame_Succeeds()
        {
            var country = countries[0];
            var entryPoint = GetTestEntryOrExitPoint(country, new Guid("E0B04105-8E22-49E6-A00C-CBD2F2D11B54"));
            var exitPoint = GetTestEntryOrExitPoint(country, new Guid("E0B04105-8E22-49E6-A00C-CBD2F2D11B54"));
            var transitState = new TransitState(country, GetTestCompetentAuthority(country), entryPoint, exitPoint, 1);

            Assert.Equal(new Guid("E0B04105-8E22-49E6-A00C-CBD2F2D11B54"), transitState.EntryPoint.Id);
            Assert.Equal(new Guid("E0B04105-8E22-49E6-A00C-CBD2F2D11B54"), transitState.ExitPoint.Id);
        }

        //[Theory]
        //[InlineData("50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "AAC022B3-ECE8-4BFE-9D69-216B3F4C13D0")]
        //[InlineData("50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "AAC022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0")]
        //[InlineData("50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "AAC022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0")]
        //[InlineData("AAC022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0")]
        //[InlineData("AAC022B3-ECE8-4BFE-9D69-216B3F4C13D0", "AAC022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0", "50C022B3-ECE8-4BFE-9D69-216B3F4C13D0")]
        //public void TransitState_ParametersNotInTheSameCountry_Throws(string countryId, string competentAuthorityId, string entryPointId, string exitPointId)
        //{
        //    var country = countries[0];
        //    var competentAuthority = GetTestCompetentAuthority(countries[1]);
        //    var entryPoint = GetTestEntryOrExitPoint(countries[1]);
        //    var exitPoint = GetTestEntryOrExitPoint(countries[1]);

        //    Assert.Throws<InvalidOperationException>(
        //        () => new TransitState(country, competentAuthority, entryPoint, exitPoint, 1));
        //}

        [Fact]
        public void AddTransitState_OnlyTransitState_AddedSuccessfully()
        {
            var country = countries[1];

            var transitState = new TransitState(country,
                GetTestCompetentAuthority(country),
                GetTestEntryOrExitPoint(country, Guid.Empty),
                GetTestEntryOrExitPoint(country, new Guid("E8E2D79A-F1CA-4928-9ED8-AAF961E2B7B7")),
                1);

            transportRoute.AddTransitStateToNotification(transitState);

            Assert.True(transportRoute.TransitStates.Count() == 1);
        }

        [Fact]
        public void AddTransitState_ToAlreadyOccupiedPosition_Throws()
        {
            var firstTransitState = new TransitState(countries[0],
                GetTestCompetentAuthority(countries[0]),
                GetTestEntryOrExitPoint(countries[0], guids[1]),
                GetTestEntryOrExitPoint(countries[0], guids[2]),
                1);

            transportRoute.AddTransitStateToNotification(firstTransitState);

            var secondTransitState = new TransitState(countries[1],
                GetTestCompetentAuthority(countries[1]),
                GetTestEntryOrExitPoint(countries[1], guids[3]),
                GetTestEntryOrExitPoint(countries[1], guids[4]),
                1);

            Assert.Throws<InvalidOperationException>(
                () => transportRoute.AddTransitStateToNotification(secondTransitState));
        }

        [Fact]
        public void AddTransitState_ToPositionOutOfRange_Throws()
        {
            var firstTransitState = new TransitState(countries[0],
                GetTestCompetentAuthority(countries[0]),
                GetTestEntryOrExitPoint(countries[0], guids[1]),
                GetTestEntryOrExitPoint(countries[0], guids[2]),
                1);

            transportRoute.AddTransitStateToNotification(firstTransitState);

            var thirdTransitState = new TransitState(countries[1],
                GetTestCompetentAuthority(countries[1]),
                GetTestEntryOrExitPoint(countries[1], guids[3]),
                GetTestEntryOrExitPoint(countries[1], guids[4]),
                3);

            Assert.Throws<InvalidOperationException>(() => transportRoute.AddTransitStateToNotification(thirdTransitState));
        }

        [Fact]
        public void AddTransitState_ToAvailablePosition_AddsToNotification()
        {
            var firstCountry = countries[0];
            var secondCountry = countries[1];

            var firstTransitState = new TransitState(firstCountry,
                GetTestCompetentAuthority(firstCountry),
                GetTestEntryOrExitPoint(firstCountry, guids[1]),
                GetTestEntryOrExitPoint(firstCountry, guids[2]),
                1);

            transportRoute.AddTransitStateToNotification(firstTransitState);

            var secondTransitState = new TransitState(secondCountry,
                GetTestCompetentAuthority(secondCountry),
                GetTestEntryOrExitPoint(secondCountry, guids[3]),
                GetTestEntryOrExitPoint(secondCountry, guids[4]),
                2);

            transportRoute.AddTransitStateToNotification(secondTransitState);

            Assert.Equal(2, transportRoute.TransitStates.Count());
        }

        [Fact]
        public void AddTransitState_ToThirdPosition_AddsToNotification()
        {
            var firstCountry = countries[0];
            var secondCountry = countries[1];
            var thirdCountry = countries[2];

            var firstTransitState = new TransitState(firstCountry,
                GetTestCompetentAuthority(firstCountry),
                GetTestEntryOrExitPoint(firstCountry, guids[1]),
                GetTestEntryOrExitPoint(firstCountry, guids[2]),
                1);

            transportRoute.AddTransitStateToNotification(firstTransitState);

            var secondTransitState = new TransitState(secondCountry,
                GetTestCompetentAuthority(secondCountry),
                GetTestEntryOrExitPoint(secondCountry, guids[3]),
                GetTestEntryOrExitPoint(secondCountry, guids[4]),
                2);

            transportRoute.AddTransitStateToNotification(secondTransitState);

            var thirdTransitState = new TransitState(thirdCountry,
                GetTestCompetentAuthority(thirdCountry),
                GetTestEntryOrExitPoint(thirdCountry, guids[5]),
                GetTestEntryOrExitPoint(thirdCountry, guids[6]),
                3);

            transportRoute.AddTransitStateToNotification(thirdTransitState);

            Assert.Equal(3, transportRoute.TransitStates.Count());
        }

        [Fact]
        public void AddTransitState_ReturnedInOrdinalOrder()
        {
            // Arrange
            var firstCountry = countries[0];
            var secondCountry = countries[1];
            var thirdCountry = countries[2];

            var thirdTransitState = new TransitState(thirdCountry,
                                                    GetTestCompetentAuthority(thirdCountry),
                                                    GetTestEntryOrExitPoint(thirdCountry, guids[5]),
                                                    GetTestEntryOrExitPoint(thirdCountry, guids[6]),
                                                    3);

            transportRoute.AddTransitStateToNotification(thirdTransitState);

            var firstTransitState = new TransitState(firstCountry,
                                                    GetTestCompetentAuthority(firstCountry),
                                                    GetTestEntryOrExitPoint(firstCountry, guids[1]),
                                                    GetTestEntryOrExitPoint(firstCountry, guids[2]),
                                                    1);

            transportRoute.AddTransitStateToNotification(firstTransitState);

            var secondTransitState = new TransitState(secondCountry,
                                                    GetTestCompetentAuthority(secondCountry),
                                                    GetTestEntryOrExitPoint(secondCountry, guids[3]),
                                                    GetTestEntryOrExitPoint(secondCountry, guids[4]),
                                                    2);

            // Act
            transportRoute.AddTransitStateToNotification(secondTransitState);

            // Assert
            Assert.Equal(3, transportRoute.TransitStates.Count());
            var actualStates = transportRoute.TransitStates.GetEnumerator();
            actualStates.MoveNext();
            Assert.Equal(1, actualStates.Current.OrdinalPosition);
            actualStates.MoveNext();
            Assert.Equal(2, actualStates.Current.OrdinalPosition);
            actualStates.MoveNext();
            Assert.Equal(3, actualStates.Current.OrdinalPosition);
        }

        [Fact]
        public void GetAvailableTransitStatePositions_WhereNotificationIsEmpty_ReturnsOne()
        {
            int[] result = transportRoute.GetAvailableTransitStatePositions();

            Assert.Equal(1, result[0]);
        }

        [Fact]
        public void GetAvailableTransitStatePositions_WhereNotificationHasOneState_ReturnsTwo()
        {
            var firstCountry = countries[0];

            var firstTransitState = new TransitState(firstCountry,
                GetTestCompetentAuthority(firstCountry),
                GetTestEntryOrExitPoint(firstCountry, guids[1]),
                GetTestEntryOrExitPoint(firstCountry, guids[2]),
                1);

            transportRoute.AddTransitStateToNotification(firstTransitState);

            int[] result = transportRoute.GetAvailableTransitStatePositions();

            Assert.Equal(2, result[0]);
        }

        [Fact]
        public void GetAvailableTransitStatePositions_WhereNotificationHasTwoStates_ReturnsThree()
        {
            var firstCountry = countries[0];
            var secondCountry = countries[1];

            var firstTransitState = new TransitState(firstCountry,
                GetTestCompetentAuthority(firstCountry),
                GetTestEntryOrExitPoint(firstCountry, guids[1]),
                GetTestEntryOrExitPoint(firstCountry, guids[2]),
                1);

            transportRoute.AddTransitStateToNotification(firstTransitState);

            var secondTransitState = new TransitState(secondCountry,
                GetTestCompetentAuthority(secondCountry),
                GetTestEntryOrExitPoint(secondCountry, guids[3]),
                GetTestEntryOrExitPoint(secondCountry, guids[4]),
                2);

            transportRoute.AddTransitStateToNotification(secondTransitState);

            int[] result = transportRoute.GetAvailableTransitStatePositions();

            Assert.Equal(3, result[0]);
        }

        [Fact]
        public void UpdateTransitState_SetToSameCountryAsExisting_AddsToNotification()
        {
            transportRoute.AddTransitStateToNotification(transitStates[0]);

            transportRoute.AddTransitStateToNotification(transitStates[1]);

            transportRoute.UpdateTransitStateForNotification(transitStates[1].Id, countries[0],
                CompetentAuthorityFactory.Create(guids[0], countries[0]),
                EntryOrExitPointFactory.Create(guids[0], countries[0]),
                EntryOrExitPointFactory.Create(guids[1], countries[0]), null);

            Assert.Equal(2, transportRoute.TransitStates.Count());
        }

        [Fact]
        public void UpdateTransitState_SetToSameOrdinalPositionAsExisting_Throws()
        {
            transportRoute.AddTransitStateToNotification(transitStates[0]);

            transportRoute.AddTransitStateToNotification(transitStates[1]);

            Assert.Throws<InvalidOperationException>(() => transportRoute.UpdateTransitStateForNotification(transitStates[1].Id, countries[1],
                CompetentAuthorityFactory.Create(guids[0], countries[1]),
                EntryOrExitPointFactory.Create(guids[0], countries[1]),
                EntryOrExitPointFactory.Create(guids[1], countries[1]), 1));
        }

        [Fact]
        public void UpdateTransitState_ValidUpdate_SetsCorrectValue()
        {
            transportRoute.AddTransitStateToNotification(transitStates[0]);

            transportRoute.AddTransitStateToNotification(transitStates[1]);

            transportRoute.UpdateTransitStateForNotification(transitStates[1].Id, countries[1],
                CompetentAuthorityFactory.Create(guids[0], countries[1]),
                EntryOrExitPointFactory.Create(guids[0], countries[1]),
                EntryOrExitPointFactory.Create(guids[2], countries[1]), null);

            Assert.Equal(guids[2], transportRoute.TransitStates.Single(ts => ts.Id == transitStates[1].Id).ExitPoint.Id);
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

        private CompetentAuthority GetTestCompetentAuthority(Country country)
        {
            var competentAuthority = ObjectInstantiator<CompetentAuthority>.CreateNew();
            ObjectInstantiator<CompetentAuthority>.SetProperty(x => x.Country, country, competentAuthority);
            return competentAuthority;
        }
    }
}
