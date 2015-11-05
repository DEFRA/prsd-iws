namespace EA.Iws.Web.Areas.ImportNotification.ViewModels.WasteType
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Core.ImportNotification.Draft;

    public class WasteTypeViewModel
    {
        public WasteTypeViewModel()
        {
        }

        public WasteTypeViewModel(WasteType data)
        {
            Name = data.Name;
        }

        public Guid ImportNotificationId { get; set; }

        [Display(Name = "WasteTypeName", ResourceType = typeof(WasteTypeViewModelResources))]
        public string Name { get; set; }
    }
}