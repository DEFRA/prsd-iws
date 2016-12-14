namespace EA.Iws.Web.Areas.Admin.ViewModels.UserAdministration
{
    using System;
    using Core.Admin;
    using Core.Authorization;

    public class ManageUserViewModel
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public UserRole AssignedRole { get; set; }

        public InternalUserStatus AssignedStatus { get; set; }

        public ManageUserViewModel()
        {
        }

        public ManageUserViewModel(InternalUserData data)
        {
            Id = data.Id;
            FullName = data.FullName;
            Email = data.Email;
            AssignedRole = data.Role;
            AssignedStatus = data.Status;
        }
    }
}