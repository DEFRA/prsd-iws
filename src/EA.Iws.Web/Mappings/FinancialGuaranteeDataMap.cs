namespace EA.Iws.Web.Mappings
{
    using Areas.Admin.ViewModels;
    using Areas.Admin.ViewModels.FinancialGuarantee;
    using Core.Admin;
    using Prsd.Core.Helpers;
    using Prsd.Core.Mapper;

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