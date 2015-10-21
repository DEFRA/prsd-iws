namespace EA.Iws.RequestHandlers.Admin.Search
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Search;
    using Core.NotificationAssessment;
    using Core.WasteType;
    using DataAccess;
    using Domain;
    using Prsd.Core.Domain;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    public class GetBasicSearchResultsHandler : IRequestHandler<GetBasicSearchResults, IList<BasicSearchResult>>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public GetBasicSearchResultsHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<IList<BasicSearchResult>> HandleAsync(GetBasicSearchResults query)
        {
            var userCompetentAuthority = await context.InternalUsers.Where(u => u.UserId == userContext.UserId.ToString()).Select(u => u.CompetentAuthority.Value).SingleAsync();
            var compAuthority = Enumeration.FromValue<UKCompetentAuthority>(userCompetentAuthority);

            return (await context.NotificationApplications
                .Join(context.Exporters, n => n.Id, e => e.NotificationId, (n, e) => new { n, e })
                .Where(p => (p.n.NotificationNumber.Contains(query.SearchTerm) ||
                            p.n.NotificationNumber.Replace(" ", string.Empty).Contains(query.SearchTerm)) ||
                            p.e.Business.Name.Contains(query.SearchTerm) &&
                            p.n.CompetentAuthority.Value == compAuthority.Value)
                .Join(context.NotificationAssessments
                    .Where(p => p.Status != NotificationStatus.NotSubmitted), j => j.n.Id,
                    na => na.NotificationApplicationId, (n, na) => new { n, na })
                .Select(
                    s =>
                        new
                        {
                            s.n.n.Id,
                            s.n.n.NotificationNumber,
                            ExporterName = s.n.e.Business.Name,
                            WasteType = (int?)s.n.n.WasteType.ChemicalCompositionType.Value,
                            s.na.Status,
                            s.n.n.CompetentAuthority
                        })
                .ToListAsync()).Select(s => ConvertToSearchResults(
                    s.Id,
                    s.NotificationNumber,
                    s.ExporterName,
                    s.WasteType,
                    s.Status)).ToList();
        }

        private static BasicSearchResult ConvertToSearchResults(Guid notificationId, string notificationNumber,
            string exporterName, int? wasteTypeValue, NotificationStatus status)
        {
            var searchResult = new BasicSearchResult
            {
                Id = notificationId,
                NotificationNumber = notificationNumber,
                ExporterName = exporterName,
                WasteType =
                    wasteTypeValue != null
                        ? Enum.GetName(typeof(ChemicalCompositionType), wasteTypeValue)
                        : string.Empty,
                NotificationStatus = EnumHelper.GetDisplayName(status)
            };

            return searchResult;
        }
    }
}