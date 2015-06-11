namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class PackagingInfo : Entity
    {
        public PackagingType PackagingType { get; private set; }

        private string otherDescription;

        protected PackagingInfo()
        {
        }

        private PackagingInfo(PackagingType packagingType)
        {
            PackagingType = packagingType;
        }

        private PackagingInfo(PackagingType packagingType, string otherDescription)
        {
            PackagingType = packagingType;
            OtherDescription = otherDescription;
        }

        public string OtherDescription
        {
            get
            {
                return otherDescription;
            }
            private set
            {
                if (PackagingType == PackagingType.Other)
                {
                    Guard.ArgumentNotNullOrEmpty(() => OtherDescription, value);
                    otherDescription = value;
                }
                else if (!string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("Cannot set other description when packaging type is not 'other'");
                }
            }
        }

        public static PackagingInfo CreatePackagingInfo(PackagingType packagingType)
        {
            if (packagingType == PackagingType.Other)
            {
                throw new InvalidOperationException("Use CreateOtherPackagingInfo factory method to create a packaging info of type 'Other'");
            }
            return new PackagingInfo(packagingType);
        }

        public static PackagingInfo CreateOtherPackagingInfo(string otherDescription)
        {
            return new PackagingInfo(PackagingType.Other, otherDescription);
        }
    }
}