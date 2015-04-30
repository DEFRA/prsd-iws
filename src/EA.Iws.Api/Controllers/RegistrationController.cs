namespace EA.Iws.Api.Controllers
{
    using Client.Entities;
    using Core.Cqrs;
    using Core.Domain;
    using Cqrs.Organisations;
    using Cqrs.Registration;
    using Cqrs.Users;
    using Domain;
    using Identity;
    using Microsoft.AspNet.Identity;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    [RoutePrefix("api/Registration")]
    public class RegistrationController : ApiController
    {
        private readonly ApplicationUserManager userManager;
        private readonly IQueryBus queryBus;
        private readonly ICommandBus commandBus;
        private readonly IUserContext userContext;

        public RegistrationController(ApplicationUserManager userManager,
            IQueryBus queryBus,
            ICommandBus commandBus,
            IUserContext userContext)
        {
            this.userManager = userManager;
            this.queryBus = queryBus;
            this.commandBus = commandBus;
            this.userContext = userContext;
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

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone,
                FirstName = model.FirstName,
                Surname = model.Surname
            };

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

            //await commandBus.SendAsync(new CreateOrganisation(organisation));

            return Ok();
        }

        [HttpGet]
        //[Authorize]
        [Route("OrganisationSearch/{organisationName}")]
        public async Task<OrganisationData[]> OrganisationSearch(string organisationName)
        {
            if (string.IsNullOrEmpty(organisationName))
            {
                return null;
            }

            var user = await queryBus.QueryAsync(new UserById(userContext.UserId));

            // Users which have already been through the search should not be permitted to run another search.
            if (user.Organisation != null)
            {
                return null;
            }

            IList<Organisation> organisations = await queryBus.QueryAsync(new FindMatchingOrganisations(organisationName));

            var apiOrganisatons = organisations.Select(o => new OrganisationData
            {
                Name = o.Name,
                Id = o.Id,
                Address = new AddressData
                {
                    Building = o.Address.Building,
                    StreetOrSuburb = o.Address.StreetOrSuburb,
                    TownOrCity = o.Address.TownOrCity,
                    Region = o.Address.Region,
                    Country = o.Address.Country.Name,
                    PostalCode = o.Address.PostalCode
                }
            }).ToArray();

            return apiOrganisatons;
        }

        [HttpPost]
        [Authorize]
        [Route("OrganisationSelect")]
        public async Task<IHttpActionResult> OrganisationSelect(OrganisationLinkData organisationLink)
        {
            var organisation = await queryBus.QueryAsync(new OrganisationById(organisationLink.OrganisationId));

            if (organisation == null)
            {
                return BadRequest();
            }

            await commandBus.SendAsync(new LinkUserToOrganisation(userContext.UserId.ToString(), organisation));

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
                        //We are using the email address as the username so this avoids duplicate validation error message
                        if (!error.StartsWith("Name"))
                        {
                            ModelState.AddModelError(string.Empty, error);
                        }
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