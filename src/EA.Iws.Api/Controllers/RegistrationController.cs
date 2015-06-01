namespace EA.Iws.Api.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using Client.Entities;
    using DataAccess.Identity;
    using Identity;
    using Microsoft.AspNet.Identity;
    using Prsd.Core.Domain;

    [RoutePrefix("api/Registration")]
    public class RegistrationController : ApiController
    {
        private readonly ApplicationUserManager userManager;
        private readonly IUserContext userContext;

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