namespace EA.Iws.Web.ViewModels.Registration
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Shared;

    public class ChangeAccountDetailsViewModel
    {
        [Required(ErrorMessage = "Please answer this question.")]
        public bool? ChangeOptions { get; set; }
    }
}