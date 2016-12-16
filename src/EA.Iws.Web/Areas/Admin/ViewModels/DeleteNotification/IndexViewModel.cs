namespace EA.Iws.Web.Areas.Admin.ViewModels.DeleteNotification
{
    using System.ComponentModel.DataAnnotations;

    public class IndexViewModel
    {
        [Required(ErrorMessageResourceName = "NumberRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        [Display(Name = "NotificationNumber", ResourceType = typeof(IndexViewModelResources))]
        public string NotificationNumber { get; set; }

        [Required(ErrorMessageResourceName = "NotificationChoiceRequired", ErrorMessageResourceType = typeof(IndexViewModelResources))]
        public bool? IsExportNotification { get; set; }
    }
}