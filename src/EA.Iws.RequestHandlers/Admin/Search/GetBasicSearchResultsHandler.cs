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
    using Prsd.Core.Helpers;
    using Prsd.Core.Mediator;
    using Requests.Admin;

    public class GetBasicSearchResultsHandler : IRequestHandler<GetBasicSearchResults, IList<BasicSearchResult>>
    {
        private readonly IwsContext context;

        public GetBasicSearchResultsHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IList<BasicSearchResult>> HandleAsync(GetBasicSearchResults query)
        {
            return (await context.NotificationApplications
                .Where(p => p.NotificationNumber.Contains(query.SearchTerm) ||
                            p.NotificationNumber.Replace(" ", string.Empty).Contains(query.SearchTerm) ||
                            p.Exporter.Business.Name.Contains(query.SearchTerm))
                .Join(context.NotificationAssessments
                    .Where(p => p.Status != NotificationStatus.NotSubmitted), n => n.Id,
                    na => na.NotificationApplicationId, (n, na) => new { n, na })
                .Select(
                    s =>
                        new
                        {
                            s.n.Id,
                            s.n.NotificationNumber,
                            ExporterName = s.n.Exporter.Business.Name,
                            WasteType = (int?)s.n.WasteType.ChemicalCompositionType.Value,
                            s.na.Status
                        })
                .ToListAsync())
                .Select(s => ConvertToSearchResults(
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