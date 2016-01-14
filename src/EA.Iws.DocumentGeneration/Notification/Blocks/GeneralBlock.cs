namespace EA.Iws.DocumentGeneration.Notification.Blocks
{
    using System.Collections.Generic;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Formatters;
    using Mapper;
    using ViewModels;

    internal class GeneralBlock : IDocumentBlock
    {
        private readonly GeneralViewModel data;

        public GeneralBlock(IList<MergeField> mergeFields, NotificationApplication notification, ShipmentInfo shipmentInfo, FacilityCollection facilityCollection)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, "General");

            data = new GeneralViewModel(notification, 
                shipmentInfo,
                facilityCollection,
                new DateTimeFormatter(), 
                new QuantityFormatter(),
                new PhysicalCharacteristicsFormatter());
        }

        public string TypeName
        {
            get { return "General"; }
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; private set; }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(GeneralViewModel));

            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }
        }

        public int OrdinalPosition
        {
            get { return 3; }
        }
    }
}