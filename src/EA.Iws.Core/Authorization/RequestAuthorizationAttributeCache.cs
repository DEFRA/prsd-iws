namespace EA.Iws.Core.Authorization
{
    using System;
    using System.Collections.Concurrent;
    using System.Reflection;

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