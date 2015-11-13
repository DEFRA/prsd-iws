namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Prsd.Core;
    using RequestHandlers.Movement;
    using TestHelpers.Helpers;
    using Xunit;

    public class RecordMovementStatusChangeTest
    {
        private const string AnyString = "test";
        private static readonly Guid AnyGuid = new Guid("78679031-FAFA-4323-A16E-A92FB947EB43");
        private readonly RecordMovementStatusChange handler;
        private readonly Movement movement;
        private readonly Guid notificationId;
        private readonly Guid userId;
        private readonly MovementStatusChangeEvent receivedEvent;
        private readonly TestIwsContext context;

        public RecordMovementStatusChangeTest()
        {
            notificationId = new Guid("1BDB59A9-349E-43E2-9D81-51955FDBF735");
            userId = TestIwsContext.UserId;

            //TODO: create ObjectInstantator<T>.CreateInstance(params) method...
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var culture = CultureInfo.InvariantCulture;
            var parameters = new object[] { 1, notificationId, new DateTime(2015, 1, 1) };
            movement = (Movement)Activator.CreateInstance(
                typeof(Movement), flags, null, parameters, culture);

            context = new TestIwsContext();
            context.Users.Add(UserFactory.Create(userId, AnyString, AnyString, AnyString, AnyString));

            var userContext = new TestUserContext(userId);
            handler = new RecordMovementStatusChange(context, userContext);

            receivedEvent = new MovementStatusChangeEvent(movement, MovementStatus.Submitted);
        }

        [Fact]
        public async Task AddsStatusChangeRecord()
        {
            var currentRecordCount = movement.StatusChanges.Count();

            await handler.HandleAsync(receivedEvent);

            Assert.Equal(currentRecordCount + 1, movement.StatusChanges.Count());
        }

        [Fact]
        public async Task AddsRecordWithCorrectStatus()
        {
            Predicate<MovementStatusChange> submittedStatusChanges =
                m => m.Status == MovementStatus.Submitted;

            Assert.DoesNotContain(movement.StatusChanges, submittedStatusChanges);

            await handler.HandleAsync(receivedEvent);

            Assert.Contains(movement.StatusChanges, submittedStatusChanges);
        }

        [Fact]
        public async Task AddsRecordWithCorrectDate()
        {
            SystemTime.Freeze();

            var date = SystemTime.UtcNow;

            await handler.HandleAsync(receivedEvent);

            Assert.Equal(date,
                movement.StatusChanges
                    .Single(sc => sc.Status == MovementStatus.Submitted)
                    .ChangeDate);
        }

        [Fact]
        public async Task AddsRecordWithCorrectUser()
        {
            await handler.HandleAsync(receivedEvent);

            Assert.Equal(userId.ToString(),
                movement.StatusChanges
                    .Single(sc => sc.Status == MovementStatus.Submitted)
                    .User.Id);
        }

        [Fact]
        public async Task CallsSaveChanges()
        {
            await handler.HandleAsync(receivedEvent);

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}