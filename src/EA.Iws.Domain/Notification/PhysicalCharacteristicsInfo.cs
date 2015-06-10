namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core.Domain;

    public class PhysicalCharacteristicsInfo : Entity
    {
        private string otherDescription;

        protected PhysicalCharacteristicsInfo()
        {
        }

        internal PhysicalCharacteristicsInfo(PhysicalCharacteristicType physicalCharacteristic)
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
    }
}