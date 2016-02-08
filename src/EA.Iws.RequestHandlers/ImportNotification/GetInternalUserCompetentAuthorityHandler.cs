namespace EA.Iws.RequestHandlers.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Notification;
    using Core.Shared;
    using Domain;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotification;

    internal class GetInternalUserCompetentAuthorityHandler : IRequestHandler<GetInternalUserCompetentAuthority, Tuple<UKCompetentAuthority, CompetentAuthorityData>>
    {
        private readonly IInternalUserRepository internalUserRepository;
        private readonly ICountryRepository countryRepository;
        private readonly ICompetentAuthorityRepository competentAuthorityRepository;
        private readonly IMapper mapper;
        private readonly IUserContext userContext;

        private static readonly Dictionary<UKCompetentAuthority, string> AuthorityCodes = new Dictionary<UKCompetentAuthority, string>
        {
            { UKCompetentAuthority.England, "GB01" },
            { UKCompetentAuthority.Scotland, "GB02" },
            { UKCompetentAuthority.NorthernIreland, "GB03" },
            { UKCompetentAuthority.Wales, "GB04" }
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

        public async Task<Tuple<UKCompetentAuthority, CompetentAuthorityData>> HandleAsync(GetInternalUserCompetentAuthority message)
        {
            var user = await internalUserRepository.GetByUserId(userContext.UserId);

            var userAuthority = user.CompetentAuthority;

            var countryId = await countryRepository.GetUnitedKingdomId();

            var lookupAuthority = await competentAuthorityRepository.GetCompetentAuthorities(countryId);

            return new Tuple<UKCompetentAuthority, CompetentAuthorityData>(userAuthority,
                mapper.Map<CompetentAuthorityData>(
                    lookupAuthority.Single(ca => ca.Code == AuthorityCodes[userAuthority])));
        }
    }
}