namespace EA.Iws.RequestHandlers.Copy
{
    using System.Linq;
    using Core.ComponentRegistration;
    using Domain.TransportRoute;

    [AutoRegister]
    internal class TransportRouteToTransportRouteCopy
    {
        public virtual void CopyTransportRoute(TransportRoute source, TransportRoute destination)
        {
            CopyStateOfExport(source, destination);
            CopyStateOfImport(source, destination);
            CopyTransitStates(source, destination);
            CopyCustomsOffices(source, destination);
        }

        public virtual void CopyTransportRouteWithoutExport(TransportRoute source, TransportRoute destination)
        {
            CopyStateOfImport(source, destination);
            CopyTransitStates(source, destination);
            CopyCustomsOffices(source, destination);
        }

        protected virtual void CopyStateOfExport(TransportRoute source, TransportRoute destination)
        {
            destination.SetStateOfExportForNotification(new StateOfExport(source.StateOfExport.Country,
                source.StateOfExport.CompetentAuthority,
                source.StateOfExport.ExitPoint));
        }

        protected virtual void CopyStateOfImport(TransportRoute source, TransportRoute destination)
        {
            destination.SetStateOfImportForNotification(new StateOfImport(source.StateOfImport.Country,
                source.StateOfImport.CompetentAuthority,
                source.StateOfImport.EntryPoint));
        }

        protected virtual void CopyTransitStates(TransportRoute source, TransportRoute destination)
        {
            foreach (var transitState in source.TransitStates.OrderBy(ts => ts.OrdinalPosition))
            {
                destination.AddTransitStateToNotification(new TransitState(transitState.Country,
                    transitState.CompetentAuthority,
                    transitState.EntryPoint,
                    transitState.ExitPoint,
                    transitState.OrdinalPosition));
            }
        }

        protected virtual void CopyCustomsOffices(TransportRoute source, TransportRoute destination)
        {
            if (source.EntryCustomsOffice != null)
            {
                destination.SetEntryCustomsOffice(new EntryCustomsOffice(source.EntryCustomsOffice.Name,
                    source.EntryCustomsOffice.Address,
                    source.EntryCustomsOffice.Country));
            }

            if (source.ExitCustomsOffice != null)
            {
                destination.SetExitCustomsOffice(new ExitCustomsOffice(source.ExitCustomsOffice.Name,
                    source.ExitCustomsOffice.Address,
                    source.ExitCustomsOffice.Country));
            }
        }
    }
}