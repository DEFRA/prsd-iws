namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.FinancialGuarantee;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetFinancialGuaranteeDecisionsHandler :
        IRequestHandler<GetFinancialGuaranteeDecisions, IEnumerable<FinancialGuaranteeDecisionData>>
    {
        private readonly IMapper mapper;
        private readonly IFinancialGuaranteeDecisionRepository repository;

        public GetFinancialGuaranteeDecisionsHandler(IFinancialGuaranteeDecisionRepository repository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<FinancialGuaranteeDecisionData>> HandleAsync(GetFinancialGuaranteeDecisions message)
        {
            var decisions = await repository.GetByNotificationId(message.NotificationId);

            return decisions.Select(x => mapper.Map<FinancialGuaranteeDecisionData>(x)).ToArray();
        }
    }
}