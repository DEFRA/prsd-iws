namespace EA.Iws.Core.Cqrs
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class AuthorizationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> inner;
        private readonly IResourceAuthorizationManager manager;
        private readonly ClaimsPrincipal principal;

        public AuthorizationCommandHandlerDecorator(ICommandHandler<TCommand> inner,
            IResourceAuthorizationManager manager, ClaimsPrincipal principal)
        {
            this.inner = inner;
            this.manager = manager;
            this.principal = principal;
        }

        public async Task HandleAsync(TCommand command)
        {
            var permissionAttribute = typeof(TCommand).GetCustomAttribute<ResourceAuthorizeAttribute>();
            bool hasAccess;

            if (permissionAttribute == null)
            {
                // TODO - throw exception or allow commands with no permissions?
                hasAccess = true;
            }
            else
            {
                var context = new ResourceAuthorizationContext(principal,
                    new[] { ActionFromAttribute(permissionAttribute) }, ResourcesFromAttribute(permissionAttribute));
                hasAccess = await manager.CheckAccessAsync(context);
            }

            if (hasAccess)
            {
                await inner.HandleAsync(command);
            }

            throw new SecurityException("Access denied.");
        }

        private Claim ActionFromAttribute(ResourceAuthorizeAttribute attribute)
        {
            return !string.IsNullOrWhiteSpace(attribute.Action) ? new Claim("name", attribute.Action) : null;
        }

        private List<Claim> ResourcesFromAttribute(ResourceAuthorizeAttribute attribute)
        {
            if ((attribute.Resources != null) && (attribute.Resources.Any()))
            {
                return attribute.Resources.Select(r => new Claim("name", r)).ToList();
            }

            return null;
        }
    }
}