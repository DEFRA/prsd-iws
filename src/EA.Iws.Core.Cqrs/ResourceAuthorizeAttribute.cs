namespace EA.Iws.Core.Cqrs
{
    using System;

    public class ResourceAuthorizeAttribute : Attribute
    {
        public ResourceAuthorizeAttribute()
        {
        }

        public ResourceAuthorizeAttribute(string action, params string[] resources)
        {
            Action = action;
            Resources = resources;
        }

        public string Action { get; private set; }
        public string[] Resources { get; private set; }
    }
}