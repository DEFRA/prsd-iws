namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Documents;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Domain;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Formatters;
    using Movement;
    using Break = DocumentFormat.OpenXml.Wordprocessing.Break;
    using Run = DocumentFormat.OpenXml.Wordprocessing.Run;

    public class MovementDocumentGenerator : IMovementDocumentGenerator
    {
        private readonly ICarrierRepository carrierRepository;
        private readonly IMovementDetailsRepository movementDetailsRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly MovementBlocksFactory blocksFactory;

        public MovementDocumentGenerator(ICarrierRepository carrierRepository,
            IMovementDetailsRepository movementDetailsRepository,
            INotificationApplicationRepository notificationApplicationRepository,
            MovementBlocksFactory blocksFactory)
        {
            this.blocksFactory = blocksFactory;
            this.movementDetailsRepository = movementDetailsRepository;
            this.carrierRepository = carrierRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<byte[]> Generate(Guid movementId)
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared("MovementMergeTemplate.docx"))
            {
                using (var document = WordprocessingDocument.Open(memoryStream, true))
                {
                    var movementDetails = await movementDetailsRepository.GetByMovementId(movementId);
                    var carrierCollection = await carrierRepository.GetByMovementId(movementId);
                    bool hasCarrierAnnex = carrierCollection.Carriers.Count() > 1;

                    var fields = MergeFieldLocator.GetMergeRuns(document);
                    var blocks = await blocksFactory.GetBlocks(movementId, fields);

                    var movementDocument = new MovementDocument(blocks);

                    ShipmentQuantityUnitFormatter.ApplyStrikethroughFormattingToUnits(document, movementDetails.ActualQuantity.Units);

                    movementDocument.Merge(hasCarrierAnnex);

                    MergeFieldLocator.RemoveDataSourceSettingFromMergedDocument(document);
                }

                return memoryStream.ToArray();
            }
        }

        public async Task<byte[]> GenerateMultiple(Guid[] movementIds)
        {
            var docs = new List<byte[]>();
            foreach (var id in movementIds)
            {
                var doc = await Generate(id);
                docs.Add(doc);
            }
            return CombineDocuments(docs);
        }

        public async Task<byte[]> GenerateBulkUploadTemplate(Guid notificationId, BulkType bulkType)
        {
            var templateName = string.Empty;

            switch (bulkType)
            {
                case BulkType.Prenotification:
                    templateName = "BulkUploadPrenotificationTemplate.xlsx";
                    break;
                case BulkType.ReceiptRecovery:
                    templateName = "BulkUploadReceiptRecoveryTemplate.xlsx";
                    break;
            }

            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared(templateName))
            {
                var notificatioNumber = await notificationApplicationRepository.GetNumber(notificationId);

                using (var document = SpreadsheetDocument.Open(memoryStream, true))
                {
                    var workbookPart = document.WorkbookPart;
                    var sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault();

                    if (sheet != null)
                    {
                        var worksheetPart = workbookPart.GetPartById(sheet.Id.Value) as WorksheetPart;

                        if (worksheetPart != null)
                        {
                            var cell = GetCell(worksheetPart.Worksheet, "A", 2);

                            cell.CellValue = new CellValue(notificatioNumber);
                            cell.DataType = new EnumValue<CellValues>(CellValues.String);

                            worksheetPart.Worksheet.Save();
                        }
                    }
                }

                return memoryStream.ToArray();
            }
        }

        private static byte[] CombineDocuments(IList<byte[]> documents)
        {
            var newBody = new Body();

            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(documents[0], 0, documents[0].Length);
                memoryStream.Position = 0;

                using (var document = WordprocessingDocument.Open(memoryStream, true))
                {
                    newBody.Append(document.MainDocumentPart.Document.Body.ChildElements.Select(e => (OpenXmlElement)e.Clone()));

                    for (int pointer = 1; pointer < documents.Count; pointer++)
                    {
                        using (var tempMemoryStream = new MemoryStream(documents[pointer]))
                        {
                            newBody.Append(new Paragraph(new Run(new Break { Type = BreakValues.Page })));

                            using (var tempDocument = WordprocessingDocument.Open(tempMemoryStream, true))
                            {
                                newBody.Append(tempDocument.MainDocumentPart.Document.Body.ChildElements.Select(e => (OpenXmlElement)e.Clone()));
                            }
                        }
                    }

                    document.MainDocumentPart.Document.Body = newBody;
                    document.MainDocumentPart.Document.Save();
                    document.Package.Flush();
                }

                return memoryStream.ToArray();
            }
        }

        private static void UpdateNotificationNumber(Stream stream, string notificatioNumber)
        {
            using (var document = SpreadsheetDocument.Open(stream, true))
            {
                var workbookPart = document.WorkbookPart;
                var sheet = workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault();

                if (sheet != null)
                {
                    var worksheetPart = workbookPart.GetPartById(sheet.Id.Value) as WorksheetPart;

                    if (worksheetPart != null)
                    {
                        var cell = GetCell(worksheetPart.Worksheet, "A", 2);

                        cell.CellValue = new CellValue(notificatioNumber);
                        cell.DataType = new EnumValue<CellValues>(CellValues.String);

                        worksheetPart.Worksheet.Save();
                    }
                }
            }
        }

        private static Cell GetCell(Worksheet worksheet,
            string columnName, uint rowIndex)
        {
            var row = worksheet.GetFirstChild<SheetData>().Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);

            if (row == null)
            {
                return null;
            }

            var firstRow =
                row.Elements<Cell>()
                    .FirstOrDefault(
                        c =>
                            string.Compare(c.CellReference.Value, columnName + rowIndex,
                                StringComparison.OrdinalIgnoreCase) == 0);

            return firstRow;
        }
    }
}
