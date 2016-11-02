namespace EA.Iws.Requests.Admin.KeyDates
{
    using Core.Admin.KeyDates;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanOverrideKeyDates)]
    public class SetImportKeyDatesOverride : IRequest<Unit>
    {
        public SetImportKeyDatesOverride(KeyDatesOverrideData data)
        {
            Data = data;
        }

        public KeyDatesOverrideData Data { get; private set; }
    }
}