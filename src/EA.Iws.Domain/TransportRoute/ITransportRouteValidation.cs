namespace EA.Iws.Domain.TransportRoute
{
    public interface ITransportRouteValidator
    {
        bool IsImportAndExportStatesCombinationValid(StateOfImport importState, StateOfExport exportState);
    }
}
