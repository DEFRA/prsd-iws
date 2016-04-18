namespace EA.Iws.Web.ViewModels.Registration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Organisations;

    public class SelectOrganisationViewModel
    {
        public string Name { get; set; }

        public IList<OrganisationData> Organisations { get; set; }

        [Required]
        public Guid? Selected { get; set; }
    }
}