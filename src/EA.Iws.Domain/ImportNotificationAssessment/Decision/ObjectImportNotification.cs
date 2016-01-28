namespace EA.Iws.Domain.ImportNotificationAssessment.Decision
{
    using System;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using ImportNotification;

    [AutoRegister]
    public class ObjectImportNotification
    {
        private readonly IImportNotificationAssessmentRepository assessmentRepository;
        private readonly IImportObjectionRepository objectionRepository;

        public ObjectImportNotification(IImportNotificationAssessmentRepository assessmentRepository, 
            IImportObjectionRepository objectionRepository)
        {
            this.assessmentRepository = assessmentRepository;
            this.objectionRepository = objectionRepository;
        }

        public async Task<ImportObjection> Object(Guid notificationId, DateTime date, string reasons)
        {
            var assessment = await assessmentRepository.GetByNotification(notificationId);

            var objection = assessment.Object(date, reasons);

            objectionRepository.Add(objection);

            return objection;
        }
    }
}
