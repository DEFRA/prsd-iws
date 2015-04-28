namespace EA.Iws.Api.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using Client.Entities;
    using Core.Cqrs;
    using Cqrs;
    using Cqrs.Registration;
    using Domain;
    using Identity;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;

    [RoutePrefix("api/Registration")]
    public class RegistrationController : ApiController
    {
        private readonly IAuthenticationManager authManager;
        private readonly ApplicationUserManager userManager;

        private readonly IQueryBus queryBus;
        private readonly ICommandBus commandBus;

        public RegistrationController(ApplicationUserManager userManager,
            IAuthenticationManager authManager)
        {
            this.userManager = userManager;
            this.authManager = authManager;
        }

        public RegistrationController(IQueryBus queryBus, ICommandBus commandBus)
        {
            this.queryBus = queryBus;
            this.commandBus = commandBus;
        }

        // POST api/Registration/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(ApplicantRegistrationData model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Registration/Register
        [AllowAnonymous]
        [Route("RegisterOrganisation")]
        public async Task<IHttpActionResult> RegisterOrganisation(OrganisationRegistrationData orgRegData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var address = new Address(orgRegData.Address1, orgRegData.TownOrCity, orgRegData.Postcode, new Country());
            var organisation = new Organisation(orgRegData.Name, address, orgRegData.EntityType);

            await commandBus.SendAsync(new CreateOrganisation(organisation));

            return Ok();
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}