namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.Reports
{
    using System;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Reports;
    using Domain;
    using Domain.Reports;
    using FakeItEasy;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using RequestHandlers.Admin.Reports;
    using Requests.Admin.Reports;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetProducerReportHandlerTests
    {
        private readonly GetProducerReportHandler handler;
        private readonly IProducerRepository repository;
        private readonly IInternalUserRepository internalUserRepo;
        private readonly Guid userId;
        private readonly UKCompetentAuthority competentAuthority = UKCompetentAuthority.England;

        public GetProducerReportHandlerTests()
        {
            userId = Guid.NewGuid();

            var userContext = A.Fake<IUserContext>();
            repository = A.Fake<IProducerRepository>();
            internalUserRepo = A.Fake<IInternalUserRepository>();

            A.CallTo(() => userContext.UserId).Returns(userId);
            A.CallTo(() => internalUserRepo.GetByUserId(userId))
                .Returns(new TestableInternalUser()
                {
                    UserId = userId.ToString(),
                    CompetentAuthority = competentAuthority
                });

            handler = new GetProducerReportHandler(repository, userContext, internalUserRepo);
        }

        [Fact]
        public async Task GetProducerReportHandler_HandleAsync_CallsInternalUserRepo()
        {
            await handler.HandleAsync(GetRequest());

            A.CallTo(() => internalUserRepo.GetByUserId(A<Guid>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task GetProducerReportHandler_HandleAsync_GetProducerReport()
        {
            var request = GetRequest();

            await handler.HandleAsync(request);

            A.CallTo(
                    () =>
                        repository.GetProducerReport(request.DateType, request.From, request.To, request.TextFieldType,
                            request.OperatorType, request.TextSearch, competentAuthority))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        private static GetProducerReport GetRequest()
        {
            return new GetProducerReport(ProducerReportDates.CompletedDate, SystemTime.UtcNow, SystemTime.UtcNow,
                ProducerReportTextFields.NotifierName, TextFieldOperator.Contains, "Test");
        }
    }
}
