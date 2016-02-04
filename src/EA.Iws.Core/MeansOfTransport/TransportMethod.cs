namespace EA.Iws.Core.MeansOfTransport
{
    using System.ComponentModel.DataAnnotations;

    public enum TransportMethod
    {
        [Display(Name = "Road", ShortName = "R")]
        Road = 1,

        [Display(Name = "Train / rail", ShortName = "T")]
        Train = 2,

        [Display(Name = "Sea", ShortName = "S")]
        Sea = 3,

        [Display(Name = "Air", ShortName = "A")]
        Air = 4,

        [Display(Name = "Inland waterways", ShortName = "W")]
        InlandWaterways = 5
    }
}