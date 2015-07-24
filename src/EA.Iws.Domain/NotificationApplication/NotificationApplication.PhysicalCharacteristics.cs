namespace EA.Iws.Domain.NotificationApplication
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