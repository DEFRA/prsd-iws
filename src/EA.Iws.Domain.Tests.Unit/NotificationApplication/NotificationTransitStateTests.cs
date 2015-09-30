namespace EA.Iws.Domain.Tests.Unit.NotificationApplication
{
    using System;
    using System.Linq;
    using Domain.TransportRoute;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationTransitStateTests
    {
        private readonly TransportRoute transportRoute;
        private static readonly Guid AnyGuid = new Guid("BC3F860E-7D09-446E-8535-C5C7735210C8");
        private static readonly Guid FirstTransitStateId = new Guid("2D6FB667-30F5-4118-83FF-D4D5F0312827");
        private static readonly Guid SecondTransitStateId = new Guid("FB4233A1-46A3-4D0E-BE41-72458B8129E4");
        private static readonly Guid ThirdTransitStateId = new Guid("54FD0805-D0C0-4974-944F-DCA69FC53FEB");
        private static readonly Guid MissingTransitStateId = new Guid("573DA75E-AD12-4F4B-A602-6F04712E7B8F");

        public NotificationTransitStateTests()
        {
            transportRoute = new TransportRoute(AnyGuid);

            var firstCountry = CountryFactory.Create(new Guid("A5E60A6E-D237-461F-8737-FE8190CEC1BC"));
            var secondCountry = CountryFactory.Create(new Guid("DB611B2D-2EF2-42AA-8857-B4B953D91767"));
            var thirdCountry = CountryFactory.Create(new Guid("C5C282CE-D4A6-4F81-BCD9-2518098D1D85"));

            transportRoute.AddTransitStateToNotification(TransitStateFactory.Create(FirstTransitStateId, firstCountry, 1));
            transportRoute.AddTransitStateToNotification(TransitStateFactory.Create(SecondTransitStateId, secondCountry, 2));
            transportRoute.AddTransitStateToNotification(TransitStateFactory.Create(ThirdTransitStateId, thirdCountry, 3));
        }

        [Fact]
        public void RemoveTransitState_IncorrectId_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => transportRoute.RemoveTransitState(MissingTransitStateId));
        }

        [Fact]
        public void RemoveTransitState_AtLastPosition_Removes()
        {
            transportRoute.RemoveTransitState(ThirdTransitStateId);

            Assert.Equal(new[] { FirstTransitStateId, SecondTransitStateId }, transportRoute.TransitStates.Select(ts => ts.Id));
        }

        [Fact]
        public void RemoveTransitState_AtFirstPosition_Removes()
        {
            transportRoute.RemoveTransitState(FirstTransitStateId);

            Assert.Equal(new[] { SecondTransitStateId, ThirdTransitStateId }, transportRoute.TransitStates.Select(ts => ts.Id));
        }

        [Fact]
        public void RemoveTransitState_AtFirstPosition_ChangesOrdinalPositionsCorrectly()
        {
            transportRoute.RemoveTransitState(FirstTransitStateId);

            Assert.Equal(new[] { 1, 2 }, transportRoute.TransitStates.Select(ts => ts.OrdinalPosition));
            Assert.Equal(1, transportRoute.TransitStates.Single(ts => ts.Id == SecondTransitStateId).OrdinalPosition);
            Assert.Equal(2, transportRoute.TransitStates.Single(ts => ts.Id == ThirdTransitStateId).OrdinalPosition);
        }

        [Fact]
        public void RemoveTransitState_AtSecondPosition_ChangesOrdinalPositionsCorrectly()
        {
            transportRoute.RemoveTransitState(SecondTransitStateId);

            Assert.Equal(new[] { 1, 2 }, transportRoute.TransitStates.Select(ts => ts.OrdinalPosition));
            Assert.Equal(1, transportRoute.TransitStates.Single(ts => ts.Id == FirstTransitStateId).OrdinalPosition);
            Assert.Equal(2, transportRoute.TransitStates.Single(ts => ts.Id == ThirdTransitStateId).OrdinalPosition);
        }

        [Fact]
        public void RemoveTransitState_RemoveTwice_RemovesBothItems()
        {
            transportRoute.RemoveTransitState(ThirdTransitStateId);
            transportRoute.RemoveTransitState(SecondTransitStateId);

            Assert.Equal(FirstTransitStateId, transportRoute.TransitStates.Single().Id);
        }

        [Fact]
        public void RemoveTransitState_RemoveTwiceFromFirst_ChangesOrdinalPositionCorrectly()
        {
            transportRoute.RemoveTransitState(FirstTransitStateId);
            transportRoute.RemoveTransitState(SecondTransitStateId);

            Assert.Equal(ThirdTransitStateId, transportRoute.TransitStates.Single().Id);
            Assert.Equal(1, transportRoute.TransitStates.Single().OrdinalPosition);
        }

        [Fact]
        public void RemoveTransitState_RemoveTwiceFromSecond_ChangesOrdinalPositionCorrectly()
        {
            transportRoute.RemoveTransitState(SecondTransitStateId);
            transportRoute.RemoveTransitState(FirstTransitStateId);

            Assert.Equal(ThirdTransitStateId, transportRoute.TransitStates.Single().Id);
            Assert.Equal(1, transportRoute.TransitStates.Single().OrdinalPosition);
        }

        [Fact]
        public void RemoveTransitState_RemoveTwiceFirstAndLast_ChangesOrdinalPositionCorrectly()
        {
            transportRoute.RemoveTransitState(FirstTransitStateId);
            transportRoute.RemoveTransitState(ThirdTransitStateId);

            Assert.Equal(SecondTransitStateId, transportRoute.TransitStates.Single().Id);
            Assert.Equal(1, transportRoute.TransitStates.Single().OrdinalPosition);
        }

        [Fact]
        public void RemoveTransitState_RemoveSameItemTwice_Throws()
        {
            transportRoute.RemoveTransitState(ThirdTransitStateId);

            Assert.Throws<InvalidOperationException>(() => transportRoute.RemoveTransitState(ThirdTransitStateId));
        }

        [Fact]
        public void RemoveTransitStates_RemoveAllItems_EmptiesList()
        {
            transportRoute.RemoveTransitState(FirstTransitStateId);
            transportRoute.RemoveTransitState(ThirdTransitStateId);
            transportRoute.RemoveTransitState(SecondTransitStateId);

            Assert.Empty(transportRoute.TransitStates);
        }

        [Fact]
        public void ImportAndExportInEU_EditTransitStateFromNonEUToEU_RemovesCustomsOfficeData()
        {
            transportRoute.RemoveTransitState(FirstTransitStateId);
            transportRoute.RemoveTransitState(ThirdTransitStateId);
            transportRoute.RemoveTransitState(SecondTransitStateId);

            var importCountry = CountryFactory.Create(new Guid("EFFD18F8-32F1-48FE-8513-0FD5E45EF730"));
            var exportCountry = CountryFactory.Create(new Guid("FA92F4B9-CE86-44D7-8554-23D3B07A5269"));
            var transitCountry = CountryFactory.Create(new Guid("873F6164-3DFE-481E-B372-9BD530304E70"), isEuMember: false);
            var transitCountryEu = CountryFactory.Create(new Guid("5B357BA4-B9E9-4325-AD26-DE7662F81FA6"));

            transportRoute.SetStateOfImportForNotification(
                new StateOfImport(
                    importCountry,
                    new TestableCompetentAuthority() { Country = importCountry },
                    new TestableEntryOrExitPoint() { Country = importCountry }));

            transportRoute.SetStateOfExportForNotification(new StateOfExport(
                exportCountry, 
                new TestableCompetentAuthority() { Country = exportCountry },
                new TestableEntryOrExitPoint() { Country = exportCountry }));

            var transitState = new TransitState(
                transitCountry, 
                new TestableCompetentAuthority() { Country = transitCountry }, 
                new TestableEntryOrExitPoint() { Country = transitCountry },
                new TestableEntryOrExitPoint() { Country = transitCountry },
                1);

            var transitStateId = new Guid("0BE78BE8-F666-4775-B3DA-7C058BFE4F4D");
            EntityHelper.SetEntityId(transitState, transitStateId);

            transportRoute.AddTransitStateToNotification(transitState);

            transportRoute.SetEntryCustomsOffice(new EntryCustomsOffice("entry", "entry", importCountry));
            transportRoute.SetExitCustomsOffice(new ExitCustomsOffice("exit", "exit", exportCountry));

            transportRoute.UpdateTransitStateForNotification(transitStateId, transitCountryEu, 
                new TestableCompetentAuthority() { Country = transitCountry },
                new TestableEntryOrExitPoint() { Country = transitCountry },
                new TestableEntryOrExitPoint() { Country = transitCountry },
                1);

            Assert.True(transportRoute.EntryCustomsOffice == null && transportRoute.ExitCustomsOffice == null,
                "Entry and Exit customs office are not both null");
        }

        [Fact]
        public void RemoveOnlyNonEUTransitState_RemovesCustomsOfficeData()
        {
            var importCountry = CountryFactory.Create(new Guid("EFFD18F8-32F1-48FE-8513-0FD5E45EF730"));
            var exportCountry = CountryFactory.Create(new Guid("FA92F4B9-CE86-44D7-8554-23D3B07A5269"));
            var transitCountry = CountryFactory.Create(new Guid("873F6164-3DFE-481E-B372-9BD530304E70"), isEuMember: false);

            var transitState = new TransitState(
                transitCountry,
                new TestableCompetentAuthority() { Country = transitCountry },
                new TestableEntryOrExitPoint() { Country = transitCountry },
                new TestableEntryOrExitPoint() { Country = transitCountry },
                4);

            var transitStateId = new Guid("0BE78BE8-F666-4775-B3DA-7C058BFE4F4D");
            EntityHelper.SetEntityId(transitState, transitStateId);

            transportRoute.SetStateOfImportForNotification(
                new StateOfImport(
                    importCountry,
                    new TestableCompetentAuthority() { Country = importCountry },
                    new TestableEntryOrExitPoint() { Country = importCountry }));

            transportRoute.SetStateOfExportForNotification(new StateOfExport(
                exportCountry,
                new TestableCompetentAuthority() { Country = exportCountry },
                new TestableEntryOrExitPoint() { Country = exportCountry }));

            transportRoute.AddTransitStateToNotification(transitState);

            transportRoute.SetEntryCustomsOffice(new EntryCustomsOffice("entry", "entry", importCountry));
            transportRoute.SetExitCustomsOffice(new ExitCustomsOffice("exit", "exit", exportCountry));

            transportRoute.RemoveTransitState(transitStateId);

            Assert.True(transportRoute.EntryCustomsOffice == null && transportRoute.ExitCustomsOffice == null,
                "Entry and Exit customs office are not both null");
        }
    }
}
