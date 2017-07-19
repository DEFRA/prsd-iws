namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.NotificationAssessment;
    using Domain.Movement;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using TestHelpers.DomainFakes;
    using Xunit;   

    public class CapturedMovementFactoryTests
    {
        private static readonly Guid NotificationId = new Guid("675287E3-42D3-4A58-86D4-691ECF620671");
        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private static readonly DateTime Today  = new DateTime(2017, 07, 19);
        private static readonly DateTime PastDate = Today.AddDays(-2);
        private static readonly DateTime FutureDate = Today.AddDays(2);
        private static readonly DateTime FutureSixtyDate = Today.AddDays(60);
        private static readonly DateTime FutureSixtyOneDate = Today.AddDays(61);
        private readonly ICapturedMovementFactory factory;
        private readonly IMovementNumberValidator validator;
        private readonly INotificationAssessmentRepository assessmentRepository;

        public CapturedMovementFactoryTests()
        {
            SystemTime.Freeze(Today);
            validator = A.Fake<IMovementNumberValidator>();
            assessmentRepository = A.Fake<INotificationAssessmentRepository>();

            A.CallTo(() => assessmentRepository.GetByNotificationId(NotificationId))
                .Returns(new TestableNotificationAssessment { Status = NotificationStatus.Consented });

            factory = new CapturedMovementFactory(validator, assessmentRepository, A.Fake<IUserContext>());
        }

        [Fact]
        public async Task SameNumberAlreadyExists_Throws()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(false);

            await Assert.ThrowsAsync<MovementNumberException>(() => factory.Create(NotificationId, 1, null, AnyDate, true));
        }

        [Fact]
        public async Task PrenotifiedDateNotProvided_StatusCaptured()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(true);

            var result = await factory.Create(NotificationId, 1, null, AnyDate, true);

            Assert.Equal(MovementStatus.Captured, result.Status);
        }
        
        [Fact]
        public async Task PrenotifiedDateProvided_StatusSubmitted()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(true);

            var result = await factory.Create(NotificationId, 1, AnyDate.AddDays(5), AnyDate, false);

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

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(NotificationId, 1, null, AnyDate, true));
        }
        
        [Fact]
        public async Task HasNoPrenotification_PrenotificationDateProvided_Throws()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(false);

            await Assert.ThrowsAsync<ArgumentException>("prenotificationDate", () => factory.Create(NotificationId, 1, AnyDate, AnyDate, true));
        }

        [Fact]
        public async Task PrenotificationDateCanBeInThePast()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(true);

            var result = await factory.Create(NotificationId, 51, PastDate, Today, false);

            Assert.Equal(MovementStatus.Submitted, result.Status);
        }

        [Fact]
        public async Task PrenotificationDateInfuture_Throws()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(NotificationId, 52, FutureDate, Today, false));
        }        

        [Fact]
        public async Task PrenotificationDateCanBeToday()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(true);

            var result = await factory.Create(NotificationId, 53, Today, Today, false);

            Assert.Equal(MovementStatus.Submitted, result.Status);
        }

        [Fact]
        public async Task ActualDateCanBeInThePast()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(true);

            var result = await factory.Create(NotificationId, 54, PastDate, PastDate, false);

            Assert.Equal(MovementStatus.Submitted, result.Status);
        }

        [Fact]
        public async Task ActualDateCanBeToday()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(true);

            var result = await factory.Create(NotificationId, 55, PastDate, Today, false);

            Assert.Equal(MovementStatus.Submitted, result.Status);
        }

        [Fact]
        public async Task ActualDateBeforePrenotificationDate_Throws()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(NotificationId, 56, Today, PastDate, false));
        }

        [Fact]
        public async Task ActualDateEqualToSixtydaysPrenotificationDate_Throws()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(true);
            var result = await factory.Create(NotificationId, 57, Today, FutureSixtyDate, false);

            Assert.Equal(MovementStatus.Submitted, result.Status);
        }

        [Fact]
        public async Task ActualDateGreaterThanSixtydaysPrenotificationDate_Throws()
        {
            A.CallTo(() => validator.Validate(NotificationId, A<int>.Ignored))
                .Returns(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(NotificationId, 58, Today, FutureSixtyOneDate, false));
        }
    }
}
