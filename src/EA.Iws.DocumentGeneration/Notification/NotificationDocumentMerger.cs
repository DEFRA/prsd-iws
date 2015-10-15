namespace EA.Iws.DocumentGeneration.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Blocks;

    public class NotificationDocumentMerger
    {
        private readonly IEnumerable<IDocumentBlock> notificationBlocks;
        private readonly IList<MergeField> mergeFields;

        public NotificationDocumentMerger(IList<MergeField> mergeFields, IEnumerable<IDocumentBlock> notificationBlocks)
        {
            this.mergeFields = mergeFields;
            this.notificationBlocks = notificationBlocks;
        }

        public void Merge()
        {
            foreach (var block in notificationBlocks)
            {
                block.Merge();
            }

            MergeAnnexes();
        }

        private void MergeAnnexes()
        {
            int annexNumber = 1;
            string tocText = string.Empty;
            string instructionsText = string.Empty;

            foreach (var block in notificationBlocks.OrderBy(b => b.OrdinalPosition))
            {
                var annexBlock = block as IAnnexedBlock;
                if (annexBlock != null && annexBlock.HasAnnex)
                {
                    annexBlock.GenerateAnnex(annexNumber);

                    var newTocText = string.IsNullOrEmpty(annexBlock.TocText)
                        ? string.Empty
                        : annexBlock.TocText + Environment.NewLine;
                    tocText = tocText + newTocText;

                    var newInstructionsText = string.IsNullOrEmpty(annexBlock.InstructionsText)
                        ? string.Empty
                        : annexBlock.InstructionsText + Environment.NewLine;
                    instructionsText = instructionsText + newInstructionsText;

                    annexNumber++;
                }
            }

            var finalBlock = new NumberOfAnnexesAndInstructionsAndToCBlock(mergeFields, annexNumber - 1, tocText,
                instructionsText);
            finalBlock.Merge();
        }
    }
}