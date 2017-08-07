namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Wordprocessing;
    using Domain;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Formatters;
    using Movement;

    public class MovementDocumentGenerator : IMovementDocumentGenerator
    {
        private readonly ICarrierRepository carrierRepository;
        private readonly IMovementDetailsRepository movementDetailsRepository;
        private readonly MovementBlocksFactory blocksFactory;

        public MovementDocumentGenerator(ICarrierRepository carrierRepository,
            IMovementDetailsRepository movementDetailsRepository,
            MovementBlocksFactory blocksFactory)
        {
            this.blocksFactory = blocksFactory;
            this.movementDetailsRepository = movementDetailsRepository;
            this.carrierRepository = carrierRepository;
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

        // https://stackoverflow.com/a/2463729
        private static byte[] CombineDocuments(IList<byte[]> documents)
        {
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(documents[0], 0, documents[0].Length);
                memoryStream.Position = 0;

                using (var document = WordprocessingDocument.Open(memoryStream, true))
                {
                    var newBody = XElement.Parse(document.MainDocumentPart.Document.Body.OuterXml);

                    for (int pointer = 1; pointer < documents.Count; pointer++)
                    {
                        newBody.Add(XElement.Parse(new Paragraph(new Run(new Break { Type = BreakValues.Page })).OuterXml));
                        var tempDocument = WordprocessingDocument.Open(new MemoryStream(documents[pointer]), true);
                        var tempBody = XElement.Parse(tempDocument.MainDocumentPart.Document.Body.OuterXml);

                        newBody.Add(tempBody);
                        document.MainDocumentPart.Document.Body = new Body(newBody.ToString());
                        document.MainDocumentPart.Document.Save();
                        document.Package.Flush();
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
}
