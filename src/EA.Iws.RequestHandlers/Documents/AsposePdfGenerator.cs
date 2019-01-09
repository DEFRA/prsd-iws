namespace EA.Iws.RequestHandlers.Documents
{
    using Aspose.Words;
    using Aspose.Words.Drawing;
    using System.Drawing;
    using System.IO;
    
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

        public byte[] ConvertToPreviewPdf(byte[] docxBytes)
        {
            using (var stream = new MemoryStream(docxBytes))
            using (var pdfStream = new MemoryStream())
            {
                var document = new Document(stream);
                InsertWatermark(document);
                document.Save(pdfStream, SaveFormat.Pdf);
                return pdfStream.ToArray();
            }
        }

        private void InsertWatermark(Document document)
        {
            Shape waterMark = new Shape(document, ShapeType.TextPlainText);

            waterMark.TextPath.Text = "DRAFT";

            waterMark.Width = 500;
            waterMark.Height = 100;

            waterMark.Rotation = -40;

            waterMark.Fill.Color = Color.Gray;
            waterMark.StrokeColor = Color.Gray;

            waterMark.RelativeHorizontalPosition = RelativeHorizontalPosition.Page;
            waterMark.RelativeVerticalPosition = RelativeVerticalPosition.Page;

            waterMark.VerticalAlignment = VerticalAlignment.Center;
            waterMark.HorizontalAlignment = HorizontalAlignment.Center;

            Paragraph watermarkPara = new Paragraph(document);
            watermarkPara.AppendChild(waterMark);

            foreach (Section sect in document.Sections)
            {
                InsertWatermarkIntoHeader(watermarkPara, sect, HeaderFooterType.HeaderPrimary);
                InsertWatermarkIntoHeader(watermarkPara, sect, HeaderFooterType.HeaderFirst);
                InsertWatermarkIntoHeader(watermarkPara, sect, HeaderFooterType.HeaderEven);
            }
        }

        private void InsertWatermarkIntoHeader(Paragraph watermarkPara, Section sect, HeaderFooterType headerType)
        {
            HeaderFooter header = sect.HeadersFooters[headerType];

            if (header == null)
            {
                header = new HeaderFooter(sect.Document, headerType);
                sect.HeadersFooters.Add(header);
            }

            header.AppendChild(watermarkPara.Clone(true));
        }
    }
}