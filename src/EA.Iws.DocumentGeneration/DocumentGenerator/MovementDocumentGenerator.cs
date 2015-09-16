namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using System;
    using DocumentFormat.OpenXml.Packaging;
    using Domain;
    using Domain.Movement;
    using Movement;

    public class MovementDocumentGenerator : IMovementDocumentGenerator
    {
        public byte[] Generate(Movement movement)
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared("MovementMergeTemplate"))
            {
                using (var document = WordprocessingDocument.Open(memoryStream, true))
                {
                    ApplyUnitStrikethroughFormatting(document, movement);

                    var movementDocument = new MovementDocument(movement);

                    movementDocument.Merge();
                }

                return memoryStream.ToArray();
            }
        }

        private void ApplyUnitStrikethroughFormatting(WordprocessingDocument document, Movement movement)
        {
            //TODO: Apply strike-through formatting to the units for the actual quantity field
            throw new NotImplementedException();
        }
    }
}
