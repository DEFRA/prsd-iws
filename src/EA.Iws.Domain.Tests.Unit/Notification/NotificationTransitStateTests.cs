namespace EA.Iws.Domain.Tests.Unit.Notification
{
    using System;
    using System.Linq;
    using NotificationApplication;
    using TestHelpers.Helpers;
    using Xunit;

    public class NotificationTransitStateTests
    {
        private readonly NotificationApplication notification;
        private static readonly Guid AnyGuid = new Guid("BC3F860E-7D09-446E-8535-C5C7735210C8");
        private static readonly Guid FirstTransitStateId = new Guid("2D6FB667-30F5-4118-83FF-D4D5F0312827");
        private static readonly Guid SecondTransitStateId = new Guid("FB4233A1-46A3-4D0E-BE41-72458B8129E4");
        private static readonly Guid ThirdTransitStateId = new Guid("54FD0805-D0C0-4974-944F-DCA69FC53FEB");
        private static readonly Guid MissingTransitStateId = new Guid("573DA75E-AD12-4F4B-A602-6F04712E7B8F");

        public NotificationTransitStateTests()
        {
            notification = new NotificationApplication(AnyGuid, NotificationType.Recovery, UKCompetentAuthority.England, 250);

            var firstCountry = CountryFactory.Create(new Guid("A5E60A6E-D237-461F-8737-FE8190CEC1BC"));
            var secondCountry = CountryFactory.Create(new Guid("DB611B2D-2EF2-42AA-8857-B4B953D91767"));
            var thirdCountry = CountryFactory.Create(new Guid("C5C282CE-D4A6-4F81-BCD9-2518098D1D85"));

            notification.AddTransitStateToNotification(TransitStateFactory.Create(FirstTransitStateId, firstCountry, 1));
            notification.AddTransitStateToNotification(TransitStateFactory.Create(SecondTransitStateId, secondCountry, 2));
            notification.AddTransitStateToNotification(TransitStateFactory.Create(ThirdTransitStateId, thirdCountry, 3));
        }

        [Fact]
        public void RemoveTransitState_IncorrectId_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => notification.RemoveTransitState(MissingTransitStateId));
        }

        [Fact]
        public void RemoveTransitState_AtLastPosition_Removes()
        {
            notification.RemoveTransitState(ThirdTransitStateId);

            Assert.Equal(new[] { FirstTransitStateId, SecondTransitStateId }, notification.TransitStates.Select(ts => ts.Id));
        }

        [Fact]
        public void RemoveTransitState_AtFirstPosition_Removes()
        {
            notification.RemoveTransitState(FirstTransitStateId);

            Assert.Equal(new[] { SecondTransitStateId, ThirdTransitStateId }, notification.TransitStates.Select(ts => ts.Id));
        }

        [Fact]
        public void RemoveTransitState_AtFirstPosition_ChangesOrdinalPositionsCorrectly()
        {
            notification.RemoveTransitState(FirstTransitStateId);

            Assert.Equal(new[] { 1, 2 }, notification.TransitStates.Select(ts => ts.OrdinalPosition));
            Assert.Equal(1, notification.TransitStates.Single(ts => ts.Id == SecondTransitStateId).OrdinalPosition);
            Assert.Equal(2, notification.TransitStates.Single(ts => ts.Id == ThirdTransitStateId).OrdinalPosition);
        }

        [Fact]
        public void RemoveTransitState_AtSecondPosition_ChangesOrdinalPositionsCorrectly()
        {
            notification.RemoveTransitState(SecondTransitStateId);

            Assert.Equal(new[] { 1, 2 }, notification.TransitStates.Select(ts => ts.OrdinalPosition));
            Assert.Equal(1, notification.TransitStates.Single(ts => ts.Id == FirstTransitStateId).OrdinalPosition);
            Assert.Equal(2, notification.TransitStates.Single(ts => ts.Id == ThirdTransitStateId).OrdinalPosition);
        }

        [Fact]
        public void RemoveTransitState_RemoveTwice_RemovesBothItems()
        {
            notification.RemoveTransitState(ThirdTransitStateId);
            notification.RemoveTransitState(SecondTransitStateId);

            Assert.Equal(FirstTransitStateId, notification.TransitStates.Single().Id);
        }

        [Fact]
        public void RemoveTransitState_RemoveTwiceFromFirst_ChangesOrdinalPositionCorrectly()
        {
            notification.RemoveTransitState(FirstTransitStateId);
            notification.RemoveTransitState(SecondTransitStateId);

            Assert.Equal(ThirdTransitStateId, notification.TransitStates.Single().Id);
            Assert.Equal(1, notification.TransitStates.Single().OrdinalPosition);
        }

        [Fact]
        public void RemoveTransitState_RemoveTwiceFromSecond_ChangesOrdinalPositionCorrectly()
        {
            notification.RemoveTransitState(SecondTransitStateId);
            notification.RemoveTransitState(FirstTransitStateId);

            Assert.Equal(ThirdTransitStateId, notification.TransitStates.Single().Id);
            Assert.Equal(1, notification.TransitStates.Single().OrdinalPosition);
        }

        [Fact]
        public void RemoveTransitState_RemoveTwiceFirstAndLast_ChangesOrdinalPositionCorrectly()
        {
            notification.RemoveTransitState(FirstTransitStateId);
            notification.RemoveTransitState(ThirdTransitStateId);

            Assert.Equal(SecondTransitStateId, notification.TransitStates.Single().Id);
            Assert.Equal(1, notification.TransitStates.Single().OrdinalPosition);
        }

        [Fact]
        public void RemoveTransitState_RemoveSameItemTwice_Throws()
        {
            notification.RemoveTransitState(ThirdTransitStateId);

            Assert.Throws<InvalidOperationException>(() => notification.RemoveTransitState(ThirdTransitStateId));
        }

        [Fact]
        public void RemoveTransitStates_RemoveAllItems_EmptiesList()
        {
            notification.RemoveTransitState(FirstTransitStateId);
            notification.RemoveTransitState(ThirdTransitStateId);
            notification.RemoveTransitState(SecondTransitStateId);

            Assert.Empty(notification.TransitStates);
        }
    }
}
