namespace EA.Iws.DocumentGeneration
{
    using System.Collections.Generic;

    public interface IDocumentBlock
    {
        string TypeName { get; }

        ICollection<MergeField> CorrespondingMergeFields { get; }

        void Merge();

        int OrdinalPosition { get; }
    }
}