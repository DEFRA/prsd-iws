namespace EA.Iws.RequestHandlers.Validation
{
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Autofac;
    using Core.ComponentRegistration;
    using Requests.ImportNotification.Validate;

    [AutoRegister]
    internal class Validator : IValidator
    {
        private readonly IComponentContext context;

        public Validator(IComponentContext context)
        {
            this.context = context;
        }

        public async Task<ValidationResults> ValidateAsync<TInstance>(TInstance instance)
        {
            var validator = context.Resolve<FluentValidation.IValidator<TInstance>>();

            var result = await validator.ValidateAsync(instance);

            return new ValidationResults
            {
                EntityName = GetEntityName(instance),
                Errors = result.Errors.Select(e => e.ErrorMessage)
            };
        }

        private string GetEntityName<TInstance>(TInstance instance)
        {
            var displayNameAttributes = typeof(TInstance).GetCustomAttributes(typeof(DisplayNameAttribute), false);

            if (displayNameAttributes == null || displayNameAttributes.Count() != 1)
            {
                return typeof(TInstance).Name;
            }

            return (displayNameAttributes[0] as DisplayNameAttribute).DisplayName;
        }
    }
}