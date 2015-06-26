namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class PhysicalCharacteristicsInfo : Entity
    {
        private string otherDescription;

        protected PhysicalCharacteristicsInfo()
        {
        }

        private PhysicalCharacteristicsInfo(PhysicalCharacteristicType physicalCharacteristic)
        {
            PhysicalCharacteristic = physicalCharacteristic;
        }

        public PhysicalCharacteristicType PhysicalCharacteristic { get; private set; }

        public string OtherDescription
        {
            get { return otherDescription; }
            internal set
            {
                if (PhysicalCharacteristic == PhysicalCharacteristicType.Other)
                {
                    otherDescription = value;
                }
                else if (!string.IsNullOrEmpty(value))
                {
                    throw new InvalidOperationException(
                        "Cannot set other description when physical characteristic type is not 'other'");
                }
            }
        }

        public static PhysicalCharacteristicsInfo CreatePhysicalCharacteristicsInfo(PhysicalCharacteristicType physicalCharacteristicType)
        {
            if (physicalCharacteristicType == PhysicalCharacteristicType.Other)
            {
                throw new InvalidOperationException("Use CreateOtherPhysicalCharacteristicsInfo factory method to create a physical characteristic info of type 'Other'");
            }

            return new PhysicalCharacteristicsInfo(physicalCharacteristicType);
        }

        public static PhysicalCharacteristicsInfo CreateOtherPhysicalCharacteristicsInfo(string otherDescription)
        {
            Guard.ArgumentNotNullOrEmpty(() => otherDescription, otherDescription);

            return new PhysicalCharacteristicsInfo(PhysicalCharacteristicType.Other)
            {
                OtherDescription = otherDescription
            };
        }
    }
}