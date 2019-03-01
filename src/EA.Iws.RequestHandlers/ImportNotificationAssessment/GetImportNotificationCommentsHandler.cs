namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Core.InternalComments;
    using Domain.ImportNotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class GetImportNotificationCommentsHandler : IRequestHandler<GetImportNotificationComments, ImportNotificationCommentsData>
    {
        private readonly IImportNotificationCommentRepository repository;
        private readonly IMap<ImportNotificationComment, InternalComment> mapper;

        public GetImportNotificationCommentsHandler(IImportNotificationCommentRepository repository, IMap<ImportNotificationComment, InternalComment> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ImportNotificationCommentsData> HandleAsync(GetImportNotificationComments message)
        {
            var result = await this.repository.GetComments(message.NotificationId);

            ImportNotificationCommentsData returnData = new ImportNotificationCommentsData();
            returnData.NotificationComments = MapReturnData(result);

            return returnData;
        }

        private List<InternalComment> MapReturnData(List<ImportNotificationComment> data)
        {
            List<InternalComment> result = new List<InternalComment>();

            foreach (var comment in data)
            {
                result.Add(this.mapper.Map(comment));
            }

            return result;
        }
    }
}
