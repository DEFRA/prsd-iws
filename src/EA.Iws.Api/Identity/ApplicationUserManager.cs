namespace EA.Iws.Api.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using DataAccess;
    using DataAccess.Identity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security.DataProtection;
    using RequestHandlers.Authorization;
    using Services;
    using ClaimTypes = Core.Shared.ClaimTypes;

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly ConfigurationService configurationService;
        private readonly Func<IwsContext> iwsContext;
        private readonly RequestAuthorizationClaimsProvider requestAuthorizationClaimsProvider;

        public ApplicationUserManager(IUserStore<ApplicationUser> store,
            IDataProtectionProvider dataProtectionProvider,
            ConfigurationService configurationService,
            Func<IwsContext> iwsContext,
            RequestAuthorizationClaimsProvider requestAuthorizationClaimsProvider)
            : base(store)
        {
            this.configurationService = configurationService;
            this.iwsContext = iwsContext;
            this.requestAuthorizationClaimsProvider = requestAuthorizationClaimsProvider;

            UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            PasswordValidator = new PasswordValidator
            {
                RequiredLength = 8,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true
            };

            // Configure user lockout defaults
            UserLockoutEnabledByDefault = true;
            DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });

            RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            EmailService = new EmailService();
            SmsService = new SmsService();

            UserTokenProvider =
                new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user)
        {
            SetEmailConfirmedIfRequired(user);

            return await base.CreateAsync(user);
        }

        private void SetEmailConfirmedIfRequired(ApplicationUser user)
        {
            // We only auto-verify email where the environment is set to development and the user email is valid.
            if (string.IsNullOrWhiteSpace(configurationService.CurrentConfiguration.Environment)
                || !configurationService.CurrentConfiguration.Environment.Equals("Development", StringComparison.InvariantCultureIgnoreCase)
                || string.IsNullOrWhiteSpace(user.Email)
                || !user.Email.Contains('@'))
            {
                return;
            }

            List<string> excludedDomains = new List<string>();

            if (!string.IsNullOrWhiteSpace(configurationService.CurrentConfiguration.VerificationEmailTestDomains))
            {
                // Get the domains for which email verification is still required.
                excludedDomains =
                    configurationService.CurrentConfiguration.VerificationEmailTestDomains.Split(new[] { ',' },
                        StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            int domainStarts = user.Email.LastIndexOf("@");
            var excludeThisEmail = excludedDomains.Any(d => user.Email.Substring(domainStarts).Contains(d));

            if (!excludeThisEmail)
            {
                user.EmailConfirmed = true;
            }
        }

        public override async Task<IList<Claim>> GetClaimsAsync(string userId)
        {
            var user = await Store.FindByIdAsync(userId);

            var claims = await base.GetClaimsAsync(userId);

            if (user == null)
            {
                return claims;
            }

            if (user.OrganisationId.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.OrganisationId, user.OrganisationId.Value.ToString()));
            }

            claims.Add(new Claim(System.Security.Claims.ClaimTypes.Name, string.Format("{0} {1}", user.FirstName, user.Surname)));
            claims.Add(new Claim(System.Security.Claims.ClaimTypes.Email, user.Email));

            var context = iwsContext();
            var internalUser = await context.InternalUsers.SingleOrDefaultAsync(p => p.UserId == userId);

            if (internalUser != null)
            {
                claims.Add(new Claim(ClaimTypes.InternalUserStatus, internalUser.Status.ToString()));
                claims.Add(new Claim(System.Security.Claims.ClaimTypes.Role, "internal"));
            }
            else
            {
                claims.Add(new Claim(System.Security.Claims.ClaimTypes.Role, "external"));
            }

            var requestClaims = await requestAuthorizationClaimsProvider.GetClaims(userId);

            foreach (var claim in requestClaims)
            {
                claims.Add(claim);
            }

            return claims;
        }
    }
}