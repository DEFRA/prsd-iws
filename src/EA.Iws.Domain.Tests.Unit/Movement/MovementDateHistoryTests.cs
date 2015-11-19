namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using Domain.Movement;
    using Prsd.Core;
    using Xunit;

    public class MovementDateHistoryTests
    {
        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private static readonly Guid AnyGuid = new Guid("60012AE1-EFC1-47D1-9CF2-30F5C3EBA596");

        [Fact]
        public void NewMovementDateHistorySetsChangedDateToUtcNow()
        {
            SystemTime.Freeze();

            var result = new MovementDateHistory(AnyGuid, AnyDate);

            Assert.Equal(SystemTime.UtcNow, result.DateChanged);

            SystemTime.Unfreeze();
        }

        [Fact]
        public void DefaultMovementIdThrows()
        {
            Assert.Throws<ArgumentException>(() => new MovementDateHistory(new Guid(), AnyDate));
        }

        [Fact]
        public void DefaultDateThrows()
        {
            Assert.Throws<ArgumentException>(() => new MovementDateHistory(AnyGuid, new DateTime()));
        }
    }
}