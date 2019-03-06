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
    using Prsd.Core.Domain;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mediator;
    using Requests.Admin.Search;

    public class SearchExportNotificationsHandler : IRequestHandler<SearchExportNotifications, IList<BasicSearchResult>>
    {
        private readonly IwsContext context;
        private readonly IUserContext userContext;

        public SearchExportNotificationsHandler(IwsContext context, IUserContext userContext)
        {
            this.context = context;
            this.userContext = userContext;
        }

        public async Task<IList<BasicSearchResult>> HandleAsync(SearchExportNotifications query)
        {
            //TODO: Move this to a repository!

            var userCompetentAuthority = await context.InternalUsers
                .Where(u => u.UserId == userContext.UserId.ToString())
                .Select(u => u.CompetentAuthority)
                .SingleAsync();

            var result = await context.NotificationApplications
                .Where(n => n.CompetentAuthority == userCompetentAuthority)
                .Join(context.Exporters, n => n.Id, e => e.NotificationId,
                    (n, e) => new { Notification = n, Exporter = e })
                .Where(p => (p.Notification.NotificationNumber.Contains(query.SearchTerm) ||
                             p.Notification.NotificationNumber.Replace(" ", string.Empty).Contains(query.SearchTerm)) ||
                            p.Exporter.Business.Name.Contains(query.SearchTerm))
                .Join(context.NotificationAssessments
                    .Where(p => p.Status != NotificationStatus.NotSubmitted), x => x.Notification.Id,
                    na => na.NotificationApplicationId, (n, na) => new { n.Notification, n.Exporter, Assessment = na })
                .Select(s =>
                    new
                    {
                        s.Notification.Id,
                        s.Notification.NotificationNumber,
                        ExporterName = s.Exporter.Business.Name,
                        WasteType = s.Notification.WasteType.ChemicalCompositionType,
                        s.Assessment.Status,
                        s.Notification.CompetentAuthority
                    })
                .OrderBy(x => x.NotificationNumber)
                .ToListAsync();

            return result.Select(s => ConvertToSearchResults(
                s.Id,
                s.NotificationNumber,
                s.ExporterName,
                s.WasteType,
                s.Status,
                s.Status.Equals(NotificationStatus.Consented) || s.Status.Equals(NotificationStatus.ConsentWithdrawn))).ToList();
        }

        private static BasicSearchResult ConvertToSearchResults(Guid notificationId, string notificationNumber,
            string exporterName, ChemicalComposition wasteTypeValue, NotificationStatus status, bool showSummaryLink)
        {
            var searchResult = new BasicSearchResult
            {
                Id = notificationId,
                NotificationNumber = notificationNumber,
                ExporterName = exporterName,
                WasteType = wasteTypeValue != default(ChemicalComposition)
                    ? EnumHelper.GetShortName(wasteTypeValue) : string.Empty,
                NotificationStatus = EnumHelper.GetDisplayName(status),
                ShowShipmentSummaryLink = showSummaryLink
            };

            return searchResult;
        }
    }
}