namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.FileStore;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class SetMultipleMovementFileIdHandlerTests
    {
        private readonly SetMultipleMovementFileIdHandler handler;
        private readonly IMovementRepository movementRepository;
        private readonly INotificationApplicationRepository notificationRepository;
        private readonly IFileRepository fileRepository;
        private readonly IMovementAuditRepository movementAuditRepository;

        private readonly Guid notificationId;
        private readonly Guid fileId;
        private const string AnyString = "test";
        private const string NotificatioNumber = "GB12345";
        private const string FileType = "pdf";
        private const int MovementCount = 5;

        public SetMultipleMovementFileIdHandlerTests()
        {
            var context = new TestIwsContext();
            context.Users.Add(UserFactory.Create(TestIwsContext.UserId, AnyString, AnyString, AnyString, AnyString));

            notificationId = Guid.NewGuid();
            fileId = Guid.NewGuid();
            var userId = TestIwsContext.UserId;

            movementRepository = A.Fake<IMovementRepository>();
            notificationRepository = A.Fake<INotificationApplicationRepository>();
            fileRepository = A.Fake<IFileRepository>();
            movementAuditRepository = A.Fake<IMovementAuditRepository>();
            var userContext = A.Fake<IUserContext>();

            A.CallTo(() => notificationRepository.GetById(notificationId))
                .Returns(new TestableNotificationApplication() { NotificationNumber = NotificatioNumber });

            A.CallTo(() => fileRepository.Store(A<File>.Ignored)).Returns(fileId);

            A.CallTo(() => userContext.UserId).Returns(userId);

            handler = new SetMultipleMovementFileIdHandler(context, movementRepository, notificationRepository,
                fileRepository, movementAuditRepository, userContext);
        }

        [Fact]
        public async Task SetMultipleMovementFileIdHandler_GetsMovements()
        {
            await handler.HandleAsync(GetRequest(MovementCount));

            A.CallTo(() => movementRepository.GetById(A<Guid>.Ignored))
                .MustHaveHappened(MovementCount, Times.Exactly);
        }

        [Fact]
        public async Task SetMultipleMovementFileIdHandler_StoresFile()
        {
            await handler.HandleAsync(GetRequest(MovementCount));

            A.CallTo(() => fileRepository.Store(A<File>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task SetMultipleMovementFileIdHandler_ReturnsFileId()
        {
            var response = await handler.HandleAsync(GetRequest(MovementCount));

            Assert.Equal(fileId, response);
        }

        [Fact]
        public async Task SetMultipleMovementFileIdHandler_LogsAuditAsPrenotified()
        {
            await handler.HandleAsync(GetRequest(MovementCount));

            A.CallTo(
                    () =>
                        movementAuditRepository.Add(
                            A<MovementAudit>.That.Matches(
                                m => m.NotificationId == notificationId && m.Type == (int)MovementAuditType.Prenotified)))
                .MustHaveHappened(MovementCount, Times.Exactly);
        }

        private SetMultipleMovementFileId GetRequest(int movementCount)
        {
            var movementIds = new List<Guid>();
            for (var i = 0; i < movementCount; i++)
            {
                movementIds.Add(Guid.NewGuid());
            }

            SetMovements(movementIds);
            return new SetMultipleMovementFileId(notificationId, movementIds.ToArray(), new byte[1], FileType);
        }

        private void SetMovements(IReadOnlyList<Guid> movementIds)
        {
            foreach (var id in movementIds)
            {
                var movement = new TestableMovement
                {
                    Id = id,
                    NotificationId = notificationId,
                    Status = MovementStatus.New
                };
                A.CallTo(() => movementRepository.GetById(id)).Returns(movement);
            }
        }
    }
}
