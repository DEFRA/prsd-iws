namespace EA.Iws.RequestHandlers.Tests.Unit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Authorization;
    using Core.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Xunit;

    public class RequestAuthorizationAttributeTests
    {
        [Fact(Skip = "Not yet tagged all requests")]
        public void AllRequestsRequireAuthorizationAttribute()
        {
            var unattributedRequests = typeof(CreateNotificationApplication)
                .Assembly
                .GetTypes()
                .Where(TypeImplementsRequestInterface)
                .Where(t => t.GetCustomAttribute<RequestAuthorizationAttribute>() == null);

            var message = string.Empty;

            if (unattributedRequests.Any())
            {
                message = unattributedRequests.Count() + " request" + 
                    (unattributedRequests.Count() != 1 ? "s" : string.Empty) +
                    " without [RequestAuthorizationAttribute]:\n" +
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

            var dictionary = typeof(InMemoryAuthorizationService)
                .GetField("authorizations", BindingFlags.NonPublic | BindingFlags.Static)
                .GetValue(null);

            var validPermissions = ((Dictionary<string, IList<UserRole>>)dictionary).Keys.AsEnumerable();

            var missingPermissions = permissionsUsed.Except(validPermissions);

            var message = string.Empty;

            if (missingPermissions.Any())
            {
                message = missingPermissions.Count() + " permission" +
                    (missingPermissions.Count() != 1 ? "s" : string.Empty) +
                    " not assigned to any roles in [InMemoryAuthorizationService]:\n" +
                    missingPermissions.Aggregate((a, b) => a + ",\n" + b);
            }

            Assert.False(missingPermissions.Any(), message);
        }

        private bool TypeImplementsRequestInterface(Type type)
        {
            foreach (var i in type.GetInterfaces())
            {
                if (i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
