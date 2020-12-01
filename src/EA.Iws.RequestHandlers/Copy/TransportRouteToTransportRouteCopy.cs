﻿namespace EA.Iws.RequestHandlers.Copy
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.ComponentRegistration;
    using Domain;
    using Domain.TransportRoute;

    [AutoRegister]
    internal class TransportRouteToTransportRouteCopy
    {
        public virtual void CopyTransportRoute(TransportRoute source, TransportRoute destination, IEnumerable<IntraCountryExportAllowed> intraCountryExportAlloweds)
        {
            CopyStateOfExport(source, destination, intraCountryExportAlloweds);
            CopyStateOfImport(source, destination, intraCountryExportAlloweds);
            CopyTransitStates(source, destination);
            CopyCustomsOffices(source, destination);
            CopyEntryExitCustomsOfficeSelection(source, destination);
        }

        public virtual void CopyTransportRouteWithoutExport(TransportRoute source, TransportRoute destination, IEnumerable<IntraCountryExportAllowed> intraCountryExportAlloweds)
        {
            CopyStateOfImport(source, destination, intraCountryExportAlloweds);
            CopyTransitStates(source, destination);
            CopyCustomsOffices(source, destination);
            CopyEntryExitCustomsOfficeSelection(source, destination);
        }

        protected virtual void CopyStateOfExport(TransportRoute source, TransportRoute destination, IEnumerable<IntraCountryExportAllowed> intraCountryExportAlloweds)
        {
            if (source.StateOfExport != null)
            {
                destination.SetStateOfExportForNotification(new StateOfExport(source.StateOfExport.Country,
                    source.StateOfExport.CompetentAuthority,
                    source.StateOfExport.ExitPoint),
                    intraCountryExportAlloweds);
            }
        }

        protected virtual void CopyStateOfImport(TransportRoute source, TransportRoute destination, IEnumerable<IntraCountryExportAllowed> intraCountryExportAlloweds)
        {
            if (source.StateOfImport != null)
            {
                destination.SetStateOfImportForNotification(new StateOfImport(source.StateOfImport.Country,
                    source.StateOfImport.CompetentAuthority,
                    source.StateOfImport.EntryPoint),
                    intraCountryExportAlloweds);
            }
        }

        protected virtual void CopyTransitStates(TransportRoute source, TransportRoute destination)
        {
            if (source.TransitStates != null)
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

        protected virtual void CopyEntryExitCustomsOfficeSelection(TransportRoute source, TransportRoute destination)
        {
            if (source.EntryExitCustomsOfficeSelection != null)
            {
                destination.SetEntryExitCustomsOfficeSelection(new EntryExitCustomsOfficeSelection(source.EntryExitCustomsOfficeSelection.Entry,
                    source.EntryExitCustomsOfficeSelection.Exit));
            }
        }
    }
}