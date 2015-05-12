namespace EA.Iws.Cqrs.CompetentAuthorities
{
    using System.Collections.Generic;
    using Domain;
    using Prsd.Core.Mediator;

    public class GetAllCompetentAuthorities : IRequest<IList<CompetentAuthority>>
    {
    }
}
