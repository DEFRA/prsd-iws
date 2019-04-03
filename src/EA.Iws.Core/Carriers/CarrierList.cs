namespace EA.Iws.Core.Carriers
{
    using System;

    [Serializable]
    public class CarrierList
    {
        public Guid Id { get; set; }

        public int Order { get; set; }
        public string OrderName { get; set; }
    }
}
