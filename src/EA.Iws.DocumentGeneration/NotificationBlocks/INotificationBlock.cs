namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System.Collections.Generic;

    public interface INotificationBlock
    {
        string TypeName { get; }

        ICollection<MergeField> CorrespondingMergeFields { get; }

        void Merge();

        int OrdinalPosition { get; }
    }
}