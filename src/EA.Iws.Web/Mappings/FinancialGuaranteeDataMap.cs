namespace EA.Iws.Web.Mappings
{
    using Areas.AdminExportAssessment.ViewModels.FinancialGuarantee;
    using Core.Admin;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mapper;
    using ViewModels.Shared;

    public class FinancialGuaranteeDataToDatesMap : IMap<FinancialGuaranteeData, FinancialGuaranteeDatesViewModel>
    {
        public FinancialGuaranteeDatesViewModel Map(FinancialGuaranteeData source)
        {
            return new FinancialGuaranteeDatesViewModel
            {
                Status = EnumHelper.GetDisplayName(source.Status),
                Received = new OptionalDateInputViewModel(source.ReceivedDate),
                Completed = new OptionalDateInputViewModel(source.CompletedDate),
                DecisionRequired = source.DecisionRequiredDate,
                IsRequiredEntryComplete = source.ReceivedDate.HasValue
            };
        }
    }
}