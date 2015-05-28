namespace EA.Iws.Requests.Shipment
{
    using System.ComponentModel.DataAnnotations;

    public enum PackagingType
    {
        [Display(Name = "Drum")]
        Drum = 1,
        [Display(Name = "Wooden barrel")]
        WoodenBarrel = 2,
        [Display(Name = "Jerrican")]
        Jerrican = 3,
        [Display(Name = "Box")]
        Box = 4,
        [Display(Name = "Bag")]
        Bag = 5,
        [Display(Name = "Composite packaging")]
        CompositePackaging = 6,
        [Display(Name = "Pressure receptacle")]
        PressureReceptacle = 7,
        [Display(Name = "Bulk")]
        Bulk = 8,
        [Display(Name = "Other (please specify)")]
        Other = 9
    }
}
