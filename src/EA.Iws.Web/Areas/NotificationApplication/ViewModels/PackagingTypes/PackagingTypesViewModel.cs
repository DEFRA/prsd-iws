﻿namespace EA.Iws.Web.Areas.NotificationApplication.ViewModels.PackagingTypes
{
    using System;
    using Prsd.Core.Validation;
    using Views.PackagingTypes;
    using Web.ViewModels.Shared;

    public class PackagingTypesViewModel
    {
        public CheckBoxCollectionViewModel PackagingTypes { get; set; }

        public Guid NotificationId { get; set; }

        public bool OtherSelected { get; set; }

        [RequiredIf("OtherSelected", true, ErrorMessageResourceName = "OtherDescriptionRequired", ErrorMessageResourceType = typeof(PackagingTypesResources))]
        public string OtherDescription { get; set; }
    }
}