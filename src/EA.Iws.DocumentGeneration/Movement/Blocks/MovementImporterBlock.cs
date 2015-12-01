namespace EA.Iws.DocumentGeneration.Movement.Blocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication.Importer;
    using Notification.Blocks;

    internal class MovementImporterBlock : ImporterBlock
    {
        public MovementImporterBlock(IList<MergeField> mergeFields, Importer importer)
            : base(mergeFields, importer)
        {
        }
    }
}