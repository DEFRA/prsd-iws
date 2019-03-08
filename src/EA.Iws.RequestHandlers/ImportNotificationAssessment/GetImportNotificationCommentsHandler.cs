namespace EA.Iws.RequestHandlers.ImportNotificationAssessment
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportNotificationAssessment;
    using Core.InternalComments;
    using Domain.ImportNotificationAssessment;
    using EA.Iws.Core.Admin;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;

    internal class GetImportNotificationCommentsHandler : IRequestHandler<GetImportNotificationComments, ImportNotificationCommentsData>
    {
        private readonly IImportNotificationCommentRepository repository;
        private readonly IMap<ImportNotificationComment, InternalComment> mapper;

        private const int PageSize = 10;

        public GetImportNotificationCommentsHandler(IImportNotificationCommentRepository repository, IMap<ImportNotificationComment, InternalComment> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<ImportNotificationCommentsData> HandleAsync(GetImportNotificationComments message)
        {
            var pagedComments = await this.repository.GetPagedComments(message.NotificationId, message.Type, message.PageNumber, PageSize, message.StartDate, message.EndDate, message.ShipmentNumber);
            var allComments = await this.repository.GetComments(message.NotificationId, message.Type, message.StartDate, message.EndDate, message.ShipmentNumber);

            ImportNotificationCommentsData returnData = new ImportNotificationCommentsData();
            returnData.NotificationComments = MapReturnData(pagedComments);
            returnData.NumberOfComments = await this.repository.GetTotalNumberOfComments(message.NotificationId, message.Type);
            returnData.PageSize = PageSize;
            returnData.PageNumber = message.PageNumber;
            returnData.NumberOfFilteredComments = allComments.Count;

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
