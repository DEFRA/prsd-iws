namespace EA.Iws.Api.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;

    public class PasswordPolicy : PasswordValidator
    {
        public PasswordPolicy()
        {
            RequiredLength = 8;
        }

        public override Task<IdentityResult> ValidateAsync(string item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            var result = true;

            if (string.IsNullOrWhiteSpace(item) || item.Length < RequiredLength)
            {
                result = false;
            }

            if (!item.Any(c => IsDigit(c)))
            {
                result = false;
            }

            if (!item.Any(c => IsLower(c)))
            {
                result = false;
            }

            if (!item.Any(c => IsUpper(c)))
            {
                result = false;
            }

            if (!item.Any(c => !IsLetterOrDigit(c)))
            {
                result = false;
            }

            if (!result)
            {
                return Task.FromResult(IdentityResult.Failed("Please check that your password has at least 8 characters and contains at least one upper case letter, one lower case letter, 1 special character and one number. The password cannot be the same as the email address."));
            }

            return Task.FromResult(IdentityResult.Success);
        }
    }
}