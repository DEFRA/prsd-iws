namespace EA.Iws.RequestHandlers.Decorators
{
    using System;
    using System.Reflection;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.NotificationAssessment;
    using Prsd.Core.Mediator;
    using Requests.Authorization;

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
                    throw new InvalidOperationException(string.Format("No public property NotificationId or Id found on this request object: {0}", typeof(TRequest).FullName));
                }

                var notificationId = (Guid)notificationIdProperty.GetValue(message);

                var assessment = await repository.GetByNotificationId(notificationId);

                if (!assessment.CanEditNotification)
                {
                    throw new NotificationReadOnlyException(notificationId);
                }
            }

            return await inner.HandleAsync(message);
        }
    }
}