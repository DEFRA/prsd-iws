namespace EA.Iws.Core.WasteType
{
    using System.Collections.Generic;

    public class PhysicalCharacteristicsData
    {
        public PhysicalCharacteristicsData()
        {
            PhysicalCharacteristics = new List<PhysicalCharacteristicType>();
        }

        public IList<PhysicalCharacteristicType> PhysicalCharacteristics { get; private set; }

        public string OtherDescription { get; set; }
    }
}