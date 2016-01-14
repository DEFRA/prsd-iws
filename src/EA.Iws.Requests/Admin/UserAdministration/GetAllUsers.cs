namespace EA.Iws.Requests.Admin.UserAdministration
{
    using System.Collections.Generic;
    using Core.Admin;
    using Prsd.Core.Mediator;

    public class GetAllUsers : IRequest<IEnumerable<ChangeUserData>>
    {
    }
}
