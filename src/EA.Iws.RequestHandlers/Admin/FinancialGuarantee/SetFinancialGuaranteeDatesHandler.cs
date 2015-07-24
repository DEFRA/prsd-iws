namespace EA.Iws.RequestHandlers.Admin.FinancialGuarantee
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.Admin.FinancialGuarantee;

    internal class SetFinancialGuaranteeDatesHandler : IRequestHandler<SetFinancialGuaranteeDates, bool>
    {
        private readonly IwsContext context;

        public SetFinancialGuaranteeDatesHandler(IwsContext context)
        {
            this.context = context;
        }

        public Task<bool> HandleAsync(SetFinancialGuaranteeDates message)
        {
            throw new NotImplementedException();
        }
    }
}
