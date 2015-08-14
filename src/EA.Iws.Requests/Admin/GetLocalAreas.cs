namespace EA.Iws.Requests.Admin
{
    using System.Collections.Generic;
    using Core.Admin;
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;

    [AllowUnauthorizedUser]
    public class GetLocalAreas : IRequest<List<LocalAreaData>>
    {
    }
}