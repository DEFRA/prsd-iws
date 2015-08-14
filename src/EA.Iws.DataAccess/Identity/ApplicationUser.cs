namespace EA.Iws.DataAccess.Identity
{
    using System;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public Guid? OrganisationId { get; private set; }
    }
}