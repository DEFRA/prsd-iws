namespace EA.Iws.RequestHandlers.Tests.Unit
{
    using System.Linq;
    using System.Reflection;
    using Core.Authorization;
    using Requests.Notification;
    using Xunit;

    public class RequestAuthorizationAttributeTests
    {
        [Fact]
        public void NoNamingConflicts()
        {
            var attributedRequests = typeof(CreateNotificationApplication)
                .Assembly
                .GetTypes()
                .Select(t => t.GetCustomAttribute<RequestAuthorizationAttribute>())
                .Where(a => a != null).ToArray();

            Assert.Equal(attributedRequests.Length, attributedRequests.Select(a => a.Name).Distinct().Count());
        }
    }
}
