namespace EA.Iws.Api.Client.Entities
{
    public class TokenResponse
    {
        public TokenResponse(string accessToken, string identityToken, string error, long expiresIn, string tokenType, string refreshToken)
        {
            AccessToken = accessToken;
            IdentityToken = identityToken;
            Error = error;
            ExpiresIn = expiresIn;
            TokenType = tokenType;
            RefreshToken = refreshToken;
        }

        public string AccessToken { get; private set; }

        public string IdentityToken { get; private set; }

        public string Error { get; private set; }

        public long ExpiresIn { get; private set; }

        public string TokenType { get; private set; }

        public string RefreshToken { get; private set; }
    }
}