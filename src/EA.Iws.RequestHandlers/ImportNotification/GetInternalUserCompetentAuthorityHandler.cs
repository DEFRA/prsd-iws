namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;
    using CompetentAuthority = Core.Notification.CompetentAuthority;

    internal class GetInternalUserCompetentAuthorityHandler : IRequestHandler<GetInternalUserCompetentAuthority, Tuple<CompetentAuthority, CompetentAuthorityData>>
    {
        private readonly IInternalUserRepository internalUserRepository;
        private readonly ICountryRepository countryRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly IMapper mapper;
        private readonly IUserContext userContext;

        private static readonly Dictionary<CompetentAuthority, string> AuthorityCodes = new Dictionary<CompetentAuthority, string>
        {
            { CompetentAuthority.England, "GB01" },
            { CompetentAuthority.Scotland, "GB02" },
            { CompetentAuthority.NorthernIreland, "GB03" },
            { CompetentAuthority.Wales, "GB04" }
        }; 

        public GetInternalUserCompetentAuthorityHandler(IInternalUserRepository internalUserRepository, 
            ICountryRepository countryRepository,
            ICompetentAuthorityRepository competentAuthorityRepository,
            IMapper mapper,
            IUserContext userContext)
        {
            this.internalUserRepository = internalUserRepository;
            this.countryRepository = countryRepository;
            this.competentAuthorityRepository = competentAuthorityRepository;
            this.mapper = mapper;
            this.userContext = userContext;
        }

        public async Task<Tuple<CompetentAuthority, CompetentAuthorityData>> HandleAsync(GetInternalUserCompetentAuthority message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);

            var enumAuthority = user.CompetentAuthority.AsCompetentAuthority();

            var countryId = await countryRepository.GetUnitedKingdomId();

            var lookupAuthority = await competentAuthorityRepository.GetCompetentAuthorities(countryId);

            return new Tuple<CompetentAuthority, CompetentAuthorityData>(enumAuthority,
                mapper.Map<CompetentAuthorityData>(
                    lookupAuthority.Single(ca => ca.Code == AuthorityCodes[enumAuthority])));
        }
    }
}
