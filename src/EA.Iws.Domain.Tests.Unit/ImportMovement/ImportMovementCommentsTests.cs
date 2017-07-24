namespace EA.Iws.Domain.Tests.Unit.ImportMovement
{
    using System;
    using Domain.ImportMovement;
    using Prsd.Core;
    using Xunit;

    public class ImportMovementCommentsTests : IDisposable
    {
        private static readonly Guid notificationId = new Guid("675287E3-42D3-4A58-86D4-691ECF620671");
        private static readonly DateTime Today = new DateTime(2017, 07, 19);
        private ImportMovement movement;

        public ImportMovementCommentsTests()
        {
            SystemTime.Freeze(Today);
            movement = new ImportMovement(notificationId, 52, Today);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public void CanSetComments()
        {
            movement.SetComments("testing");

            Assert.Equal("testing", movement.Comments);
        }

        [Fact]
        public void CommentsCantBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => movement.SetComments(null));
        }

        [Fact]
        public void CommentsCantBeEmpty()
        {
            Assert.Throws<ArgumentException>(() => movement.SetComments(string.Empty));
        }

        [Fact]
        public void CanSetStatsMarking()
        {
            movement.SetStatsMarking("testing");

            Assert.Equal("testing", movement.StatsMarking);
        }

        [Fact]
        public void StatsMarkingCantBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => movement.SetStatsMarking(null));
        }

        [Fact]
        public void StatsMarkingCantBeEmpty()
        {
            Assert.Throws<ArgumentException>(() => movement.SetStatsMarking(string.Empty));
        }
    }
}