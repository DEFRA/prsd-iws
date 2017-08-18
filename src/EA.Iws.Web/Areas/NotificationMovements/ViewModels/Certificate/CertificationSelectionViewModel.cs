namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Certificate
{
    using Core.Shared;
    using Prsd.Core.Validation;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CertificationSelectionViewModel
    {
        [Required(ErrorMessageResourceType = typeof(CertificationSelectionViewModelResources), ErrorMessageResourceName = "CertificateTypeRequired")]
        public CertificateType? Certificate { get; set; }

        public Guid NotificationId { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}