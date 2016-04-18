namespace EA.Iws.RequestHandlers.Documents
{
    using System.IO;
    using Aspose.Words;

    internal class AsposePdfGenerator : IPdfGenerator
    {
        public byte[] ConvertToPdf(byte[] docxBytes)
        {
            using (var stream = new MemoryStream(docxBytes))
            using (var pdfStream = new MemoryStream())
            {
                var document = new Document(stream);
                document.Save(pdfStream, SaveFormat.Pdf);
                return pdfStream.ToArray();
            }
        }
    }
}