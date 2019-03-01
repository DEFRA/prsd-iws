namespace EA.Iws.RequestHandlers.Mappings.ImportNotificationAssessment
{
    using System;
    using System.Threading.Tasks;
    using Core.InternalComments;
    using Domain;
    using Domain.ImportNotificationAssessment;
    using Prsd.Core.Mapper;

    public class ImportNotificationCommentMap : IMap<ImportNotificationComment, InternalComment>
    {
        private readonly IInternalUserRepository internalUserRepository;

        public ImportNotificationCommentMap(IInternalUserRepository internalUserRepository)
        {
            this.internalUserRepository = internalUserRepository;
        }

        public InternalComment Map(ImportNotificationComment source)
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
