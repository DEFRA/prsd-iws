namespace EA.Iws.Web.Areas.Admin.ViewModels.UserAdministration
{
    using System;
    using Core.Admin;
    using Core.Authorization;

    public class ManageUserViewModel
    {
        public string UserId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public UserRole AssignedRole { get; set; }

        public InternalUserStatus AssignedStatus { get; set; }

        public ManageUserViewModel()
        {
        }

        public ManageUserViewModel(InternalUserData data)
        {
            UserId = data.UserId;
            FullName = data.FullName;
            Email = data.Email;
            AssignedRole = data.Role;
            AssignedStatus = data.Status;
        }
    }
}