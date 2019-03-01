namespace EA.Iws.RequestHandlers.Mappings.NotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.InternalComments;
    using Domain;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mapper;

    public class NotificationCommentMap : IMap<NotificationComment, InternalComment>
    {
        private readonly IInternalUserRepository internalUserRepository;

        public NotificationCommentMap(IInternalUserRepository internalUserRepository)
        {
            this.internalUserRepository = internalUserRepository;
        }

        public InternalComment Map(NotificationComment source)
        {
            var user = Task.Run(() => internalUserRepository.GetByUserId(source.UserId)).Result.User;

            return new InternalComment()
            {
                Comment = source.Comment,
                DateAdded = source.DateAdded.UtcDateTime,
                NotificationId = source.NotificationId,
                ShipmentNumber = source.ShipmentNumber,
                Username = user.FullName,
                CommentId = source.Id
            };
        }
    }
}
