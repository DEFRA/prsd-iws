namespace EA.Iws.Cqrs.CompetentAuthorities
{
    using System.Collections.Generic;
    using Core.Cqrs;
    using Domain;

    public class GetAllCompetentAuthorities : IQuery<IList<CompetentAuthority>>
    {
    }
}
