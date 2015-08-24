namespace EA.Iws.DataAccess.Identity
{
    using System;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public Guid? OrganisationId { get; private set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string TownOrCity { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
    }
}