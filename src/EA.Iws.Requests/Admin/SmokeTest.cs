namespace EA.Iws.Requests.Admin
{
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;

    [AllowUnauthorizedUser]
    public class SmokeTest : IRequest<bool>
    {
    }
}