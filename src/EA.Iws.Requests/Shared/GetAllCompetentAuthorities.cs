namespace EA.Iws.Requests.Shared
{
    using System.Collections.Generic;
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;

    [AllowUnauthorizedUser]
    public class GetAllCompetentAuthorities : IRequest<ICollection<CompetentAuthorityData>>
    {
    }
}
