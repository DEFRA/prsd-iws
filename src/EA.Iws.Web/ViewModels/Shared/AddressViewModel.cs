namespace EA.Iws.Web.ViewModels.Shared
{
    using System.ComponentModel.DataAnnotations;

    public class AddressViewModel
    {
        [Required]
        [StringLength(50)]
        public string Building { get; set; }

        [StringLength(50)]
        public string Street { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string County { get; set; }

        [Required]
        [StringLength(10)]
        public string Postcode { get; set; }
    }
}