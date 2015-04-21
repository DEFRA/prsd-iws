namespace EA.Iws.Core.Cqrs
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FluentValidation;

    public class ValidationCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ICommandHandler<TCommand> inner;
        private readonly IEnumerable<IValidator<TCommand>> validators;

        public ValidationCommandHandlerDecorator(ICommandHandler<TCommand> inner,
            IEnumerable<IValidator<TCommand>> validators)
        {
            this.inner = inner;
            this.validators = validators;
        }

        public ValidationCommandHandlerDecorator(ICommandHandler<TCommand> inner)
            : this(inner, new List<IValidator<TCommand>>())
        {
        }

        public async Task HandleAsync(TCommand command)
        {
            foreach (var validator in validators)
            {
                await validator.ValidateAndThrowAsync(command);
            }

            await inner.HandleAsync(command);
        }
    }
}