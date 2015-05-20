namespace EA.Iws.Requests.Shared
{
    using System.ComponentModel.DataAnnotations;

    public class BusinessData
    {
        [Required]
        [Display(Name = "Organisation name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Organisation type")]
        public string EntityType { get; set; }

        public string RegistrationNumber { get; set; }

        [Display(Name = "Additional registration number")]
        public string AdditionalRegistrationNumber { get; set; }
    }
}
