namespace EA.Iws.RequestHandlers.Admin.Search
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Admin.Search;
    using Core.WasteType;
    using DataAccess;
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
            var results = await(from n in context.NotificationApplications
                                where (n.NotificationNumber.Contains(query.SearchTerm) ||
                                       n.NotificationNumber.Replace(" ", string.Empty).Contains(query.SearchTerm) ||
                                       n.Exporter.Business.Name.Contains(query.SearchTerm))
                                    select new { n.Id, n.NotificationNumber, ExporterName = n.Exporter.Business.Name, WasteType = (int?)n.WasteType.ChemicalCompositionType.Value }).ToListAsync();
            return results.Select(r => ConvertToSearchResults(r.Id, r.NotificationNumber, r.ExporterName, r.WasteType)).ToList();
        }

        private BasicSearchResult ConvertToSearchResults(Guid notificationId, string notificationNumber, string exporterName, int? wasteTypeValue)
        {
            BasicSearchResult searchResult = new BasicSearchResult
            {
                Id = notificationId,
                NotificationNumber = notificationNumber,
                ExporterName = exporterName,
                WasteType = wasteTypeValue != null ? Enum.GetName(typeof(ChemicalCompositionType), wasteTypeValue) : string.Empty
            };

            return searchResult;
        }
    }
}
