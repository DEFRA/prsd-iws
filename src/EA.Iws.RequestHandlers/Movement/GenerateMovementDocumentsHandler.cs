namespace EA.Iws.RequestHandlers.Movement
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Documents;
    using Documents;
    using Domain;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GenerateMovementDocumentsHandler : IRequestHandler<GenerateMovementDocuments, FileData>
    {
        private readonly IMovementDocumentGenerator documentGenerator;
        private readonly IPdfGenerator pdfGenerator;

        public GenerateMovementDocumentsHandler(IMovementDocumentGenerator documentGenerator, IPdfGenerator pdfGenerator)
        {
            this.documentGenerator = documentGenerator;
            this.pdfGenerator = pdfGenerator;
        }

        public GenerateMovementDocumentsHandler(IMovementDocumentGenerator documentGenerator)
        {
            this.documentGenerator = documentGenerator;
        }

        public async Task<FileData> HandleAsync(GenerateMovementDocuments message)
        {
            var docxBytes = await documentGenerator.GenerateMultiple(message.MovementIds);
            var fileName = string.Format("IWS-Movement-{0}-{1}", message.MovementIds.First(), SystemTime.UtcNow);

            if (pdfGenerator != null)
            {
                var pdfBytes = pdfGenerator.ConvertToPdf(docxBytes);
                return new FileData(fileName, FileType.Pdf, pdfBytes);
            }

            return new FileData(fileName, FileType.Docx, docxBytes);
        }
    }
}