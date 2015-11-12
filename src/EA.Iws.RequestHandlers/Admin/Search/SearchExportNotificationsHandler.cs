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
            var userCompetentAuthority = await context.InternalUsers
                .Where(u => u.UserId == userContext.UserId.ToString())
                .Select(u => u.CompetentAuthority.Value)
                .SingleAsync();

            var compAuthority = Enumeration.FromValue<UKCompetentAuthority>(userCompetentAuthority);

            var result = await context.NotificationApplications
                .Join(context.Exporters, n => n.Id, e => e.NotificationId, (n, e) => new { Notification = n, Exporter = e })
                .Where(p => p.Notification.CompetentAuthority.Value == compAuthority.Value &&
                            (p.Notification.NotificationNumber.Contains(query.SearchTerm) ||
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
                        WasteType = (int?)s.Notification.WasteType.ChemicalCompositionType.Value,
                        s.Assessment.Status,
                        s.Notification.CompetentAuthority
                    })
                .ToListAsync();
                
            return result.Select(s => ConvertToSearchResults(
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