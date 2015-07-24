namespace EA.Iws.Domain.Notification
{
    using System.Collections.Generic;

    public partial class NotificationApplication
    {
        public void SetPhysicalCharacteristics(IEnumerable<PhysicalCharacteristicsInfo> physicalCharacteristics)
        {
            PhysicalCharacteristicsCollection.Clear();

            foreach (var physicalCharacteristic in physicalCharacteristics)
            {
                PhysicalCharacteristicsCollection.Add(physicalCharacteristic);
            }
        }
    }
}