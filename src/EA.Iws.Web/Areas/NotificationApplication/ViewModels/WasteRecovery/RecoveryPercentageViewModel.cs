namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.WasteRecovery
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Notification;
    using Prsd.Core.Validation;
    using Requests.WasteRecovery;

    public class RecoveryPercentageViewModel
    {
        [Required(ErrorMessage = "Please enter a value")]
        [Range(0, 100, ErrorMessage = "The percentage (%) must be between 0 and 100")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "The percentage (%) of recoverable material must be a number with a maximum of 2 decimal places")]
        [Display(Name = "Please enter the percentage (%) of recoverable material")]
        public decimal? PercentageRecoverable { get; set; }
    }
}