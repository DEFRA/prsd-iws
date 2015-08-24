namespace EA.Iws.Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Client.Entities;
    using Core.Shared;
    using DataAccess.Identity;
    using Identity;
    using Microsoft.AspNet.Identity;
    using Prsd.Core.Domain;

    [RoutePrefix("api/Registration")]
    public class RegistrationController : ApiController
    {
        private readonly IUserContext userContext;
        private readonly ApplicationUserManager userManager;

        public RegistrationController(ApplicationUserManager userManager,
            IUserContext userContext)
        {
            this.userContext = userContext;
            this.userManager = userManager;
        }

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
                Surname = model.Surname,
                Address2 = model.Address2,
                Address1 = model.Address1,
                PostalCode = model.Postcode,
                Region = model.CountyOrProvince,
                Country = model.CountryName,
                TownOrCity = model.TownOrCity
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok(user.Id);
        }

        [AllowAnonymous]
        [Route("RegisterAdmin")]
        public async Task<IHttpActionResult> RegisterAdmin(AdminRegistrationData model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                Surname = model.Surname
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok(user.Id);
        }

        [HttpGet]
        [Route("GetUserEmailVerificationToken")]
        public async Task<string> GetUserEmailVerificationToken()
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(userContext.UserId.ToString());

            return token;
        }

        [HttpPost]
        [Route("VerifyEmail")]
        public async Task<IHttpActionResult> VerifyEmail(VerifiedEmailData model)
        {
            var result = await userManager.ConfirmEmailAsync(model.Id.ToString(), model.Code);

            return Ok(result.Succeeded);
        }

        [HttpGet]
        [Route("GetApplicantDetails")]
        public async Task<EditApplicantRegistrationData> GetApplicantDetails()
        {
            var result = await userManager.FindByIdAsync(userContext.UserId.ToString());
            return new EditApplicantRegistrationData()
            {
                Id = new Guid(result.Id),
                FirstName = result.FirstName,
                Surname = result.Surname,
                Phone = result.PhoneNumber,
                Email = result.Email
            };
        }

        [HttpPost]
        [Route("UpdateApplicantDetails")]
        public async Task<IHttpActionResult> UpdateApplicantDetails(EditApplicantRegistrationData model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = model.Id.ToString();
            var user = await userManager.FindByIdAsync(userId);
            user.FirstName = model.FirstName;
            user.Surname = model.Surname;
            user.PhoneNumber = model.Phone;

            if (!user.Email.Equals(model.Email))
            {
                //Verify user password
                var resultChangePassword = await userManager.CheckPasswordAsync(user, model.Password);
                if (!resultChangePassword)
                {
                    return BadRequest("Wrong password");
                }
                user.Email = model.Email;
                user.UserName = model.Email;
            }

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok(user.Id);
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