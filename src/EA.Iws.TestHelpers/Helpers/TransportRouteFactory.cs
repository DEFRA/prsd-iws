namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain;
    using Domain.TransportRoute;

    public class TransportRouteFactory
    {
        public static TransportRoute CreateCompleted(Guid id, Guid notificationId,
            IList<EntryOrExitPoint> entryOrExitPoints, IList<CompetentAuthority> competentAuthorities)
        {
            var transportRoute = new TransportRoute(notificationId);
            EntityHelper.SetEntityId(transportRoute, id);

            var exitPoint =
                entryOrExitPoints.OrderBy(ep => ep.Country.Name).First(ep => ep.Country.IsEuropeanUnionMember);
            var stateOfExport = new StateOfExport(exitPoint.Country,
                competentAuthorities.First(ca => ca.Country.Id == exitPoint.Country.Id), exitPoint);

            var entryPoint = entryOrExitPoints.OrderBy(ep => ep.Country.Name)
                .First(ep => ep.Country.IsEuropeanUnionMember && ep.Country.Id != exitPoint.Country.Id);
            var stateOfImport = new StateOfImport(entryPoint.Country,
                competentAuthorities.First(ca => ca.Country.Id == entryPoint.Country.Id), entryPoint);

            transportRoute.SetStateOfExportForNotification(stateOfExport);
            transportRoute.SetStateOfImportForNotification(stateOfImport);

            return transportRoute;
        }
    }
}