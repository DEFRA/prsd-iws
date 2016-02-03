namespace EA.Iws.DocumentGeneration.Formatters
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.WasteType;
    using Domain.NotificationApplication;

    public class PhysicalCharacteristicsFormatter
    {
        public string PhysicalCharacteristicsToCommaDelimitedString(IEnumerable<PhysicalCharacteristicsInfo> physicalCharacteristics)
        {
            if (physicalCharacteristics == null)
            {
                return string.Empty;
            }

            var orderedPhysicalCharacteristics = physicalCharacteristics.OrderBy(c => c.PhysicalCharacteristic).ToList();

            return string.Join(", ", orderedPhysicalCharacteristics.Select(pc => pc.PhysicalCharacteristic == PhysicalCharacteristicType.Other
                ? pc.OtherDescription
                : ((int)pc.PhysicalCharacteristic).ToString()));
        }
    }
}
