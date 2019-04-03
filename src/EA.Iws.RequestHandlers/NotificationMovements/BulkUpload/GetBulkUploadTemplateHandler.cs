namespace EA.Iws.RequestHandlers.NotificationMovements.BulkUpload
{
    using System.Threading.Tasks;
    using Domain;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.BulkUpload;

    public class GetBulkUploadTemplateHandler : IRequestHandler<GetBulkUploadTemplate, byte[]>
    {
        private readonly IMovementDocumentGenerator generator;

        public GetBulkUploadTemplateHandler(IMovementDocumentGenerator generator)
        {
            this.generator = generator;
        }

        public async Task<byte[]> HandleAsync(GetBulkUploadTemplate message)
        {
            return await generator.GenerateBulkUploadTemplate(message.NotificationId, message.BulkType);
        }
    }
}
