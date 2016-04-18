namespace EA.Iws.RequestHandlers.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Authorization;
    using Core.Authorization;
    using Prsd.Core.Mediator;
    using Prsd.Core.Security;
    using Requests.Notification;
    using Xunit;

    public class RequestAuthorizationAttributeTests
    {
        [Fact]
        public void AllRequestsRequireAuthorizationAttribute()
        {
            var unattributedRequests = typeof(CreateNotificationApplication)
                .Assembly
                .GetTypes()
                .Where(TypeImplementsIRequest)
                .Where(t => t.GetCustomAttribute<RequestAuthorizationAttribute>() == null
                    && t.GetCustomAttribute<AllowUnauthorizedUserAttribute>() == null);

            var message = string.Empty;

            if (unattributedRequests.Any())
            {
                message = unattributedRequests.Count() + " request" +
                    (unattributedRequests.Count() != 1 ? "s" : string.Empty) +
                    " without [RequestAuthorizationAttribute] or [AllowUnauthorizedUserAttribute]:\n" +
                    unattributedRequests.Select(request => request.FullName)
                        .Aggregate((a, b) => a + ",\n" + b);
            }

            Assert.False(unattributedRequests.Any(), message);
        }

        [Fact]
        public void AllPermissionsUsedExistInAuthorizationService()
        {
            var permissionsUsed = typeof(CreateNotificationApplication)
                .Assembly
                .GetTypes()
                .Select(t => t.GetCustomAttribute<RequestAuthorizationAttribute>())
                .Where(a => a != null)
                .Select(a => a.Name);

            var dictionary = typeof(InMemoryAuthorizationManager)
                .GetField("authorizations", BindingFlags.NonPublic | BindingFlags.Static)
                .GetValue(null);

            var validPermissions = ((Dictionary<string, IList<UserRole>>)dictionary).Keys.AsEnumerable();

            var missingPermissions = permissionsUsed.Except(validPermissions);

            var message = string.Empty;

            if (missingPermissions.Any())
            {
                message = missingPermissions.Count() + " permission" +
                    (missingPermissions.Count() != 1 ? "s" : string.Empty) +
                    " not assigned to any roles in [InMemoryAuthorizationManager]:\n" +
                    missingPermissions.Aggregate((a, b) => a + ",\n" + b);
            }

            Assert.False(missingPermissions.Any(), message);
        }

        private bool TypeImplementsIRequest(Type type)
        {
            foreach (var i in type.GetInterfaces())
            {
                if (TypeIsIRequest(i))
                {
                    return true;
                }
            }
            return false;
        }

        private bool TypeIsIRequest(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IRequest<>);
        }
    }
}