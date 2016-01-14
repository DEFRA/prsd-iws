namespace EA.Iws.Requests.Admin.UserAdministration
{
    using Core.Admin;
    using Prsd.Core.Mediator;

    public class GetUserById : IRequest<ChangeUserData>
    {
        public string UserId { get; private set; }

        public GetUserById(string userId)
        {
            UserId = userId;
        }
    }
}
