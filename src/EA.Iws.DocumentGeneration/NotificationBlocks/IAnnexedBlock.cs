namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System.Collections.Generic;

    internal interface IAnnexedBlock
    {
        bool HasAnnex { get; }

        void GenerateAnnex(int annexNumber);

        IList<MergeField> AnnexMergeFields { get; }

        string TocText { get; }

        string InstructionsText { get; }
    }
}
