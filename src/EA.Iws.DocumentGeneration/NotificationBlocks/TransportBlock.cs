namespace EA.Iws.DocumentGeneration.NotificationBlocks
{
    using System.Collections.Generic;
    using System.Reflection;
    using Domain.TransportRoute;
    using Mapper;
    using ViewModels;

    internal class TransportBlock : IDocumentBlock
    {
        private const string TransportFields = "Transport";
        private readonly TransportViewModel data;

        public TransportBlock(IList<MergeField> mergeFields, TransportRoute transportRoute)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new TransportViewModel(transportRoute);
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