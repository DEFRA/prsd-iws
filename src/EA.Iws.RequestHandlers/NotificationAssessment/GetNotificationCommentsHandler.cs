namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.InternalComments;
    using Core.NotificationAssessment;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    internal class GetNotificationCommentsHandler : IRequestHandler<GetNotificationComments, NotificationCommentData>
    {
        private readonly INotificationCommentRepository repository;
        private readonly IMap<NotificationComment, InternalComment> mapper;

        private const int PageSize = 10;

        public GetNotificationCommentsHandler(INotificationCommentRepository repository, IMap<NotificationComment, InternalComment> mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<NotificationCommentData> HandleAsync(GetNotificationComments message)
        {
            var pagedComments = await this.repository.GetPagedComments(message.NotificationId, message.Type, message.PageNumber, PageSize, message.StartDate, message.EndDate, message.ShipmentNumber);
            var allComments = await this.repository.GetComments(message.NotificationId, message.Type, message.StartDate, message.EndDate, message.ShipmentNumber);

            NotificationCommentData returnData = new NotificationCommentData();
            returnData.NotificationComments = MapReturnData(pagedComments);
            returnData.NumberOfComments = await this.repository.GetTotalNumberOfComments(message.NotificationId, message.Type);
            returnData.PageSize = PageSize;
            returnData.PageNumber = message.PageNumber;
            returnData.NumberOfFilteredComments = allComments.Count;

            return returnData;
        }

        private List<InternalComment> MapReturnData(List<NotificationComment> data)
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
