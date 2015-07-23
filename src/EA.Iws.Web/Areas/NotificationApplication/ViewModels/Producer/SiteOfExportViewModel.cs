namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Producer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Producers;

    public class SiteOfExportViewModel
    {
        public Guid NotificationId { get; set; }

        [Required]
        [Display(Name = "Site of export")]
        public Guid? SelectedSiteOfExport { get; set; }

        public IList<ProducerData> Producers { get; set; }

        public SiteOfExportViewModel()
        {
            Producers = new List<ProducerData>();
        }
    }
}