namespace EA.Iws.Requests.Admin
{
    using System.Collections.Generic;
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;

    [AllowUnauthorizedUser]
    public class GetAreaNames : IRequest<List<string>>
    {
    }
}