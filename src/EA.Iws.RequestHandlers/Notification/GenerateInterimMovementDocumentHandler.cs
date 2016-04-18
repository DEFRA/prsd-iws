namespace EA.Iws.RequestHandlers.Notification
{
    using System.Threading.Tasks;
    using Core.Documents;
    using Documents;
    using Domain;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.Notification;

    internal class GenerateInterimMovementDocumentHandler : IRequestHandler<GenerateInterimMovementDocument, FileData>
    {
        private readonly IInterimMovementDocumentGenerator interimMovementDocumentGenerator;

        public GenerateInterimMovementDocumentHandler(IInterimMovementDocumentGenerator interimMovementDocumentGenerator)
        {
            this.interimMovementDocumentGenerator = interimMovementDocumentGenerator;
        }
        
        public async Task<FileData> HandleAsync(GenerateInterimMovementDocument message)
        {
            var docxBytes = await interimMovementDocumentGenerator.Generate(message.NotificationId);
            var fileName = string.Format("IWS-Interim-Movement-{0}-{1}", message.NotificationId, SystemTime.UtcNow);

            return new FileData(fileName, FileType.Docx, docxBytes);
        }
    }
}
