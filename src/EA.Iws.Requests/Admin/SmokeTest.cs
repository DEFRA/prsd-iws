namespace EA.Iws.Requests.Admin
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(SystemConfigurationPermissions.CanViewSmokeTest)]
    public class SmokeTest : IRequest<bool>
    {
    }
}