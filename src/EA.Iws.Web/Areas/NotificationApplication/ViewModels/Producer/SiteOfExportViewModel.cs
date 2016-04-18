namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.Producer
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Producers;
    using Views.Producer;

    public class SiteOfExportViewModel
    {
        public Guid NotificationId { get; set; }

        [Required(ErrorMessageResourceType = typeof(SiteOfExportResources), ErrorMessageResourceName = "SiteOfExportRequired")]
        [Display(Name = "SiteOfExport", ResourceType = typeof(SiteOfExportResources))]
        public Guid? SelectedSiteOfExport { get; set; }

        public IList<ProducerData> Producers { get; set; }

        public SiteOfExportViewModel()
        {
            Producers = new List<ProducerData>();
        }
    }
}