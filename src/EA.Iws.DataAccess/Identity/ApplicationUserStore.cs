﻿namespace EA.Iws.DataAccess.Identity
{
    using Microsoft.AspNet.Identity.EntityFramework;

    public class ApplicationUserStore : UserStore<ApplicationUser>
    {
        public ApplicationUserStore(IwsIdentityContext context)
            : base(context)
        {
        }
    }
}