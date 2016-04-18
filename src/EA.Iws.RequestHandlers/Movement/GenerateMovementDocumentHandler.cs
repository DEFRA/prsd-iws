namespace EA.Iws.RequestHandlers.Movement
{
    using System.Threading.Tasks;
    using Core.Documents;
    using Documents;
    using Domain;
    using Prsd.Core;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class GenerateMovementDocumentHandler : IRequestHandler<GenerateMovementDocument, FileData>
    {
        private readonly IMovementDocumentGenerator documentGenerator;
        private readonly IPdfGenerator pdfGenerator;

        public GenerateMovementDocumentHandler(IMovementDocumentGenerator documentGenerator, IPdfGenerator pdfGenerator)
        {
            this.documentGenerator = documentGenerator;
            this.pdfGenerator = pdfGenerator;
        }

        public GenerateMovementDocumentHandler(IMovementDocumentGenerator documentGenerator)
        {
            this.documentGenerator = documentGenerator;
        }

        public async Task<FileData> HandleAsync(GenerateMovementDocument message)
        {
            var docxBytes = await documentGenerator.Generate(message.Id);
            var fileName = string.Format("IWS-Movement-{0}-{1}", message.Id, SystemTime.UtcNow);

            if (pdfGenerator != null)
            {
                var pdfBytes = pdfGenerator.ConvertToPdf(docxBytes);
                return new FileData(fileName, FileType.Pdf, pdfBytes);
            }

            return new FileData(fileName, FileType.Docx, docxBytes);
        }
    }
}