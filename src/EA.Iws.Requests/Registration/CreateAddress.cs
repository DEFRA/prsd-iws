namespace EA.Iws.Requests.Registration
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(GeneralPermissions.CanEditUserData)]
    public class CreateAddress : IRequest<Guid>
    {
        public AddressData Address { get; set; }

        public string UserId { get; set; }
    }
}
