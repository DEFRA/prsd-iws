namespace EA.Iws.Domain
{
    using Prsd.Core.Domain;
    public class PhysicalCharacteristicType : Enumeration
    {
        public static readonly PhysicalCharacteristicType Powdery = new PhysicalCharacteristicType(1, "Powdery/powder");
        public static readonly PhysicalCharacteristicType Solid = new PhysicalCharacteristicType(2, "Solid");
        public static readonly PhysicalCharacteristicType Viscous = new PhysicalCharacteristicType(3, "Viscous/paste");
        public static readonly PhysicalCharacteristicType Sludgy = new PhysicalCharacteristicType(4, "Sludgy");
        public static readonly PhysicalCharacteristicType Liquid = new PhysicalCharacteristicType(5, "Liquid");
        public static readonly PhysicalCharacteristicType Gaseous = new PhysicalCharacteristicType(6, "Gaseous");
        public static readonly PhysicalCharacteristicType Other = new PhysicalCharacteristicType(7, "Other (please specify)");
        protected PhysicalCharacteristicType()
        {
        }
        private PhysicalCharacteristicType(int value, string displayName)
            : base(value, displayName)
        {
        }
    }
}