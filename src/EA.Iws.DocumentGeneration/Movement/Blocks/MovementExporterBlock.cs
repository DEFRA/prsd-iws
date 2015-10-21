namespace EA.Iws.DocumentGeneration.Movement.Blocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication.Exporter;
    using Notification.Blocks;

    internal class MovementExporterBlock : ExporterBlock
    {
        public MovementExporterBlock(IList<MergeField> mergeFields, Exporter exporter)
            : base(mergeFields, exporter)
        {
        }
    }
}