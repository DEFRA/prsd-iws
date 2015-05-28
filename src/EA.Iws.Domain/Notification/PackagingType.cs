namespace EA.Iws.Domain
{
    using System;
    using Prsd.Core.Domain;

    public class PackagingType : Enumeration
    {
        public static readonly PackagingType Drum = new PackagingType(1, "Drum");

        public static readonly PackagingType WoodenBarrel = new PackagingType(2, "Wooden barrel");

        public static readonly PackagingType Jerrican = new PackagingType(3, "Jerrican");

        public static readonly PackagingType Box = new PackagingType(4, "Box");

        public static readonly PackagingType Bag = new PackagingType(5, "Bag");

        public static readonly PackagingType CompositePackaging = new PackagingType(6, "Composite packaging");

        public static readonly PackagingType PressureReceptacle = new PackagingType(7, "Pressure receptacle");

        public static readonly PackagingType Bulk = new PackagingType(8, "Bulk");

        public static readonly PackagingType Other = new PackagingType(9, "Other (please specify)");

        private PackagingType()
        {
        }

        private PackagingType(int value, string displayName)
            : base(value, displayName)
        {
        }

        public Guid Id { get; set; }
    }
}