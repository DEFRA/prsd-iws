namespace EA.Iws.DocumentGeneration.Movement.Blocks
{
    using System.Collections.Generic;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Formatters;
    using Mapper;
    using ViewModels;

    public class MovementBlock : IDocumentBlock
    {
        private readonly MovementViewModel data;

        public MovementBlock(IList<MergeField> mergeFields, 
            Movement movement, 
            MovementDetails movementDetails,
            NotificationApplication notification, 
            ShipmentInfo shipmentInfo)
        {
            CorrespondingMergeFields = MergeFieldLocator.GetCorrespondingFieldsForBlock(mergeFields, TypeName);
            data = new MovementViewModel(
                movement,
                movementDetails,
                notification,
                shipmentInfo,
                new DateTimeFormatter(),
                new QuantityFormatter(),
                new PhysicalCharacteristicsFormatter(),
                new PackagingTypesFormatter());
        }

        public string TypeName
        {
            get { return "Movement"; }
        }

        public ICollection<MergeField> CorrespondingMergeFields { get; set; }

        public void Merge()
        {
            var properties = PropertyHelper.GetPropertiesForViewModel(typeof(MovementViewModel));

            foreach (var field in CorrespondingMergeFields)
            {
                MergeFieldDataMapper.BindCorrespondingField(field, data, properties);
            }
        }

        public int OrdinalPosition
        {
            get { return 0; }
        }
    }
}