namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System.Collections.Generic;
    using System.Reflection;
    using Domain.NotificationApplication;
    using Mapper;
    using ViewModels;

    internal class TransportBlock : INotificationBlock
    {
        private const string TransportFields = "Transport";
        private readonly TransportViewModel data;

        public TransportBlock(IList<MergeField> mergeFields, NotificationApplication notification)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new TransportViewModel(notification);
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public string TypeName
        {
            get { return TransportFields; }
        }

        public int OrdinalPosition
        {
            get { return 15; }
        }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(TransportViewModel));
            MergeTransportToMainDocument(data, properties);
        }

        private void MergeTransportToMainDocument(TransportViewModel transport, PropertyInfo[] properties)
        {
            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, transport, properties);
            }
        }
    }
}