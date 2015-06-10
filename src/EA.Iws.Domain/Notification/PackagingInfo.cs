namespace EA.Iws.Domain.Notification
{
    using System;
    using Prsd.Core.Domain;

    public class PackagingInfo : Entity
    {
        public PackagingType PackagingType { get; private set; }

        private string otherDescription;

        protected PackagingInfo()
        {
        }

        internal PackagingInfo(PackagingType packagingType)
        {
            PackagingType = packagingType;
        }

        public string OtherDescription
        {
            get
            {
                return otherDescription;
            }
            internal set
            {
                if (PackagingType == PackagingType.Other)
                {
                    otherDescription = value;
                }
                else if (!string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidOperationException("Cannot set other description when packaging type is not 'other'");
                }
            }
        }
    }
}
