namespace EA.Iws.Requests.WasteType
{
    using System.ComponentModel.DataAnnotations;
    public enum PhysicalCharacteristicType
    {
        [Display(Name = "Powdery/powder")]
        Powdery = 1,
        [Display(Name = "Solid")]
        Solid = 2,
        [Display(Name = "Viscous/paste")]
        Viscous = 3,
        [Display(Name = "Sludgy")]
        Sludgy = 4,
        [Display(Name = "Liquid")]
        Liquid = 5,
        [Display(Name = "Gaseous")]
        Gaseous = 6,
        [Display(Name = "Other (please specify)")]
        Other = 7
    }
}