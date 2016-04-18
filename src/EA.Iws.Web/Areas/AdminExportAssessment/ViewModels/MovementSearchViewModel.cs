namespace EA.Iws.Web.Areas.AdminExportAssessment.ViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class MovementSearchViewModel
    {
        [Required]
        public string Number { get; set; }
    }
}