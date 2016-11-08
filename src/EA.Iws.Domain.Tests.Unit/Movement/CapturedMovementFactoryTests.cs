namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.NotificationAssessment;
    using Domain.Movement;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class CapturedMovementFactoryTests
    {
        private static readonly Guid NotificationId = new Guid("675287E3-42D3-4A58-86D4-691ECF620671");
        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private readonly ICapturedMovementFactory factory;
        private readonly IMovementNumberValidator validator;
        private readonly INotificationAssessmentRepository assessmentRepository;

        public CapturedMovementFactoryTests()
        {
            validator = A.Fake<IMovementNumberValidator>();
            assessmentRepository = A.Fake<INotificationAssessmentRepository>();

            A.CallTo(() => assessmentRepository.GetByNotificationId(NotificationId))
                .Returns(new TestableNotificationAssessment { Status = NotificationStatus.Consented });

            factory = new CapturedMovementFactory(validator, assessmentRepository);
        }

        [Fact]
        public async Task SameNumberAlreadyExists_Throws()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(false);

            await Assert.ThrowsAsync<MovementNumberException>(() => factory.Create(NotificationId, 1, null, AnyDate));
        }

        [Fact]
        public async Task PrenotifiedDateNotProvided_StatusCaptured()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(true);

            var result = await factory.Create(NotificationId, 1, null, AnyDate);

            Assert.Equal(MovementStatus.Captured, result.Status);
        }
        
        [Fact]
        public async Task PrenotifiedDateProvided_StatusSubmitted()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(true);

            var result = await factory.Create(NotificationId, 1, AnyDate.AddDays(5), AnyDate);

            Assert.Equal(MovementStatus.Submitted, result.Status);
            Assert.Equal(AnyDate.AddDays(5), result.PrenotificationDate);
            Assert.Equal(AnyDate, result.Date);
        }

        [Fact]
        public async Task NotificationNotConsented_Throws()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(true);

            A.CallTo(() => assessmentRepository.GetByNotificationId(NotificationId))
                .Returns(new TestableNotificationAssessment { Status = NotificationStatus.FileClosed });

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(NotificationId, 1, null, AnyDate));
        }
    }
}
