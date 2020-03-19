namespace EA.Iws.Web.Areas.Admin.ViewModels.VirusScan
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class VirusScanViewModel
    {
        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}