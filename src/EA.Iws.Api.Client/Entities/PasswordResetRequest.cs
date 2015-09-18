namespace EA.Iws.Api.Client.Entities
{
    public class PasswordResetRequest
    {
        public string EmailAddress { get; set; }

        public string Url { get; set; }
    }
}