namespace EA.Iws.Web.Areas.Admin.ViewModels.ImportNotification
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NotificationInterimViewModel
    {
        public string NotificationNumber { get; set; }

        public DateTime NotificationReceivedDate { get; set; }

        [Display(ResourceType = typeof(NotificationInterimViewModelResources), Name = "IsInterim")]
        [Required(ErrorMessageResourceType = typeof(NotificationInterimViewModelResources), ErrorMessageResourceName = "IsInterimRequired")]
        public bool? IsInterim { get; set; }
    }
}