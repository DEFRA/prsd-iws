namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Reject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Movement;

    public class RejectViewModel
    {
        public IList<MovementData> Movements { get; set; }

        [Required(ErrorMessageResourceType = typeof(RejectViewModelResources), ErrorMessageResourceName = "SelectedRequired")]
        [Display(ResourceType = typeof(RejectViewModelResources), Name = "SelectedName")]
        public Guid? Selected { get; set; }
    }
}