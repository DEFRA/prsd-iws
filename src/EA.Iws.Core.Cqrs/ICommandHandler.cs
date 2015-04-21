namespace EA.Iws.Core.Cqrs
{
    using System.Threading.Tasks;

    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}
