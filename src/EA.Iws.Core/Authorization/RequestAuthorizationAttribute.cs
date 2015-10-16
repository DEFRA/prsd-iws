namespace EA.Iws.Core.Authorization
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class RequestAuthorizationAttribute : Attribute
    {
        public string Name { get; private set; }
        
        public RequestAuthorizationAttribute()
        {
        }

        public RequestAuthorizationAttribute(string name)
        {
            Name = name;
        }
    }
}
