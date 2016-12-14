namespace EA.Iws.Web.Areas.Admin.ViewModels.UserAdministration
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.Authorization;
    using Prsd.Core.Helpers;

    public class ExistingUsersListViewModel
    {
        public SelectList GetUserRolesList(UserRole selectedValue)
        {
            return new SelectList(new[]
            {
                new SelectListItem { Value = UserRole.Internal.ToString(), Text = EnumHelper.GetDisplayName(UserRole.Internal) },
                new SelectListItem { Value = UserRole.Administrator.ToString(), Text = EnumHelper.GetDisplayName(UserRole.Administrator) },
            }, "Value", "Text", selectedValue);
        }

        public SelectList GetUserStatusList(InternalUserStatus selectedValue)
        {
            return new SelectList(new[]
            {
                new KeyValuePair<string, InternalUserStatus>(EnumHelper.GetDisplayName(InternalUserStatus.Approved), InternalUserStatus.Approved),
                new KeyValuePair<string, InternalUserStatus>(EnumHelper.GetDisplayName(InternalUserStatus.Inactive), InternalUserStatus.Inactive)
            }, "Value", "Key", selectedValue);
        }

        public IList<ManageUserViewModel> Users { get; set; }

        public ExistingUsersListViewModel()
        {
            Users = new List<ManageUserViewModel>();
        }
    }
}