namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.Reports;
    using Core.Reports.Producer;

    public interface IProducerRepository
    {
        Task<ProducerCollection> GetByNotificationId(Guid notificationId);

        Task<ProducerCollection> GetByMovementId(Guid movementId);

        void Add(ProducerCollection producerCollection);

        Task<IEnumerable<ProducerData>> GetProducerReport(ProducerReportDates dateType, DateTime from, DateTime to,
            ProducerReportTextFields? textFieldType, TextFieldOperator? operatorType, string textSearch,
            UKCompetentAuthority competentAuthority);
    }
}