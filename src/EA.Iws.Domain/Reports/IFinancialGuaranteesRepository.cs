namespace EA.Iws.Domain.Reports
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;

    public interface IFinancialGuaranteesRepository
    {
        Task<IEnumerable<FinancialGuaranteesData>> GetBlanketBonds(UKCompetentAuthority competentAuthority, string financialGuaranteeReferenceNumber, string exporterName,
            string importerName, string producerName);
    }
}