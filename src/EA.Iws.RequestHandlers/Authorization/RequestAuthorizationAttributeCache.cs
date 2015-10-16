namespace EA.Iws.RequestHandlers.Authorization
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;
    using Core.Authorization;

    public class RequestAuthorizationAttributeCache
    {
        private readonly ConcurrentDictionary<Type, RequestAuthorizationAttribute> cache
            = new ConcurrentDictionary<Type, RequestAuthorizationAttribute>();

        public RequestAuthorizationAttribute Get(Type t)
        {
            return cache.GetOrAdd(t, GetAttributeForType);
        }

        private RequestAuthorizationAttribute GetAttributeForType(Type t)
        {
            return t.GetCustomAttribute<RequestAuthorizationAttribute>();
        }
    }
}