namespace EA.Iws.RequestHandlers
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests;

    internal class NotificationReadOnlyAuthorizeDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> inner;
        private readonly INotificationAssessmentRepository repository;

        public NotificationReadOnlyAuthorizeDecorator(IRequestHandler<TRequest, TResponse> inner, INotificationAssessmentRepository repository)
        {
            this.inner = inner;
            this.repository = repository;
        }

        public async Task<TResponse> HandleAsync(TRequest message)
        {
            var readOnlyAttribute = typeof(TRequest).GetCustomAttribute<NotificationReadOnlyAuthorizeAttribute>();

            if (readOnlyAttribute != null)
            {
                // Try and get the notification id
                var notificationIdProperty = typeof(TRequest).GetProperty("NotificationId") ??
                                             typeof(TRequest).GetProperty("Id");

                if (notificationIdProperty == null)
                {
                    throw new InvalidOperationException("Request does not contain a property for notification id");
                }

                var notificationId = (Guid)notificationIdProperty.GetValue(message);

                var assessment = await repository.GetByNotificationId(notificationId);

                if (!assessment.CanEditNotification)
                {
                    throw new InvalidOperationException(string.Format("Can't edit notification {0} as it is read-only", notificationId));
                }
            }

            return await inner.HandleAsync(message);
        }
    }
}