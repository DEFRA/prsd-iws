namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.BulkUpload
{
    using System;
    using System.Threading.Tasks;
    using Core.Documents;
    using Domain;
    using FakeItEasy;
    using RequestHandlers.NotificationMovements.BulkUpload;
    using Requests.NotificationMovements.BulkUpload;
    using Xunit;

    public class GetBulkUploadTemplateHandlerTests
    {
        private readonly GetBulkUploadTemplateHandler handler;
        private readonly IMovementDocumentGenerator documentGenerator;
        private const int FileSize = 100;
        private readonly Guid notificationId;

        public GetBulkUploadTemplateHandlerTests()
        {
            notificationId = Guid.NewGuid();
            documentGenerator = A.Fake<IMovementDocumentGenerator>();
            A.CallTo(() => documentGenerator.GenerateBulkUploadTemplate(notificationId, A<BulkType>.Ignored)).Returns(new byte[FileSize]);
            handler = new GetBulkUploadTemplateHandler(documentGenerator);
        }

        [Fact]
        public async Task GenerateTemplateReturnsExpectedSize()
        {
            var result = await handler.HandleAsync(new GetBulkUploadTemplate(notificationId, BulkType.Prenotification));

            Assert.Equal(FileSize, result.Length);
        }

        [Fact]
        public async Task GenerateTemplateCallsGenerator()
        {
            await handler.HandleAsync(new GetBulkUploadTemplate(notificationId, BulkType.Prenotification));

            A.CallTo(() => documentGenerator.GenerateBulkUploadTemplate(notificationId, BulkType.Prenotification))
                .MustHaveHappened(Repeated.Exactly.Once);
        }
    }
}
