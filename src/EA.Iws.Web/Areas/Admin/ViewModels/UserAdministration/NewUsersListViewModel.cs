namespace EA.Iws.Web.Areas.Admin.ViewModels.UserAdministration
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.Authorization;
    using Prsd.Core.Helpers;

    public class NewUsersListViewModel : IValidatableObject
    {
        public NewUsersListViewModel()
        {
            Users = new List<UserApprovalViewModel>();
        }

        public IList<UserApprovalViewModel> Users { get; set; }

        public SelectList ApprovalActionsList
        {
            get { return new SelectList(EnumHelper.GetValues(typeof(ApprovalAction)), "Key", "Value"); }
        }

        public SelectList UserRolesList
        {
            get
            {
                return new SelectList(new[]
                {
                    new KeyValuePair<string, UserRole>(EnumHelper.GetDisplayName(UserRole.Internal), UserRole.Internal),
                    new KeyValuePair<string, UserRole>(EnumHelper.GetDisplayName(UserRole.Administrator), UserRole.Administrator),
                    new KeyValuePair<string, UserRole>(EnumHelper.GetDisplayName(UserRole.ReadOnly), UserRole.ReadOnly)
                }, "Value", "Key");
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Users.Count > 0 && Users.All(u => u.Action == null))
            {
                yield return new ValidationResult("Please select at least one action to perform");
            }

            var invalidUsers = Users.Where(u => u.Action == ApprovalAction.Approve && !u.AssignedRole.HasValue);

            foreach (var user in invalidUsers)
            {
                yield return
                    new ValidationResult("Please assign a role to an approved user",
                        new[] { string.Format("Users[{0}]", Users.IndexOf(user)) });
            }
        }
    }
}