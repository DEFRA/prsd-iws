namespace EA.Iws.DocumentGeneration.Movement.Blocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using Notification.Blocks;

    internal class MovementExporterBlock : ExporterBlock
    {
        public MovementExporterBlock(IList<MergeField> mergeFields, NotificationApplication notification)
            : base(mergeFields, notification)
        {
        }
    }
}