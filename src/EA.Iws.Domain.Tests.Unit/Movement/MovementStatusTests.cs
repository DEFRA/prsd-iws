namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Linq;
    using Core.Movement;
    using Domain.Movement;
    using Xunit;

    public class MovementStatusTests
    {
        private readonly Movement movement;
        private readonly Guid notificationId;
        private static readonly Guid AnyGuid = new Guid("B68806E3-524E-476E-A505-40B717B3191E");

        public MovementStatusTests()
        {
            notificationId = new Guid("EAD34BEE-E962-4D4D-9D53-ADCD7240C333");
            movement = new Movement(1, notificationId);
        }

        [Fact]
        public void DefaultStatusIsNew()
        {
            Assert.Equal(MovementStatus.New, movement.Status);
        }

        [Fact]
        public void SubmitChangesStatusToSubmitted()
        {
            movement.Submit(AnyGuid);

            Assert.Equal(MovementStatus.Submitted, movement.Status);
        }

        [Fact]
        public void SubmitRaisesStatusChangedEvent()
        {
            movement.Submit(AnyGuid);

            Assert.Equal(movement,
                movement.Events.OfType<MovementStatusChangeEvent>()
                    .SingleOrDefault()
                    .Movement);
        }
    }
}
