namespace EA.Iws.DocumentGeneration.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Blocks.Factories;

    public class NotificationBlocksFactory
    {
        private readonly IEnumerable<INotificationBlockFactory> blockFactories;

        public NotificationBlocksFactory(IEnumerable<INotificationBlockFactory> blockFactories)
        {
            this.blockFactories = blockFactories;
        }

        public async Task<List<IDocumentBlock>> GetBlocks(Guid notificationId, IList<MergeField> mergeFields)
        {
            var blocks = new List<IDocumentBlock>();
            foreach (var factory in blockFactories)
            {
                blocks.Add(await factory.Create(notificationId, mergeFields));
            }
            return blocks;
        }
    }
}