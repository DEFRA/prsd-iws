namespace EA.Iws.Core.Admin
{
    using System;

    public class ExternalUserData
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public ExternalUserStatus Status { get; set; }
    }
}