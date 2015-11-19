namespace EA.Iws.RequestHandlers.Admin.NotificationAssessment
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using DataAccess;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin.NotificationAssessment;

    internal class GetDatesHandler : IRequestHandler<GetDates, NotificationDatesData>
    {
        private readonly IwsContext context;
        private readonly IMapWithParameter<NotificationDates, Guid, NotificationDatesData> mapper;
        private readonly DecisionRequiredBy decisionRequiredBy;
        private readonly INotificationAssessmentRepository notificationAssessmentRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly INotificationTransactionRepository notificationTransactionRepository;
        private readonly NotificationTransactionCalculator transactionCalculator;
        private readonly INotificationChargeCalculator chargeCalculator;

        public GetDatesHandler(IwsContext context, 
            IMapWithParameter<NotificationDates, Guid, NotificationDatesData> mapper,
            DecisionRequiredBy decisionRequiredBy,
            INotificationAssessmentRepository notificationAssessmentRepository,
            INotificationApplicationRepository notificationApplicationRepository,
            INotificationTransactionRepository notificationTransactionRepository,
            NotificationTransactionCalculator transactionCalculator,
            INotificationChargeCalculator chargeCalculator)
        {
            this.context = context;
            this.mapper = mapper;
            this.decisionRequiredBy = decisionRequiredBy;
            this.notificationAssessmentRepository = notificationAssessmentRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.notificationTransactionRepository = notificationTransactionRepository;
            this.transactionCalculator = transactionCalculator;
            this.chargeCalculator = chargeCalculator;
        }

        public async Task<NotificationDatesData> HandleAsync(GetDates message)
        {
            var assessment = await notificationAssessmentRepository.GetByNotificationId(message.NotificationId);
            var notification = await notificationApplicationRepository.GetById(message.NotificationId);

            var datesData = mapper.Map(assessment.Dates, message.NotificationId);

            datesData.DecisionRequiredDate = decisionRequiredBy.GetDecisionRequiredByDate(notification, assessment);

            var tranactions = await notificationTransactionRepository.GetTransactions(message.NotificationId);
            var totalBillable = await GetTotalBillable(message.NotificationId, notification);
            var latestPayment = tranactions.Where(t => t.Credit > 0).OrderByDescending(t => t.Date).FirstOrDefault();
            var totalPaid = transactionCalculator.TotalCredits(tranactions) - transactionCalculator.TotalDebits(tranactions);

            if (latestPayment != null)
            {
                datesData.PaymentReceivedDate = latestPayment.Date;
            }

            datesData.PaymentIsComplete = totalPaid >= totalBillable;

            return datesData;
        }

        private async Task<decimal> GetTotalBillable(Guid id, NotificationApplication notification)
        {
            var pricingStructures = await context.PricingStructures.ToArrayAsync();
            var shipmentInfo = await context.ShipmentInfos.Where(s => s.NotificationId == id).FirstAsync();

            return chargeCalculator.GetValue(pricingStructures, notification, shipmentInfo);
        }
    }
}