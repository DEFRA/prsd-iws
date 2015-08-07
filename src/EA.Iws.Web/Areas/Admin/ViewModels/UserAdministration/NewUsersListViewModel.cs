namespace EA.Iws.Web.Areas.Admin.ViewModels.UserAdministration
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Admin;
    using Prsd.Core.Helpers;

    public class NewUsersListViewModel : IValidatableObject
    {
        public IList<UserApprovalViewModel> Users { get; set; }

        public SelectList ApprovalActionsList
        {
            get { return new SelectList(EnumHelper.GetValues(typeof(ApprovalAction)), "Key", "Value"); }
        }

        public NewUsersListViewModel()
        {
            Users = new List<UserApprovalViewModel>();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Users.Count > 0 && Users.All(u => u.Action == null))
            {
                yield return new ValidationResult("Please select at least one action to perform");
            }
        }
    }
}