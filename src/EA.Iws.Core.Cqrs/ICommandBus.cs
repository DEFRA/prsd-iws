namespace EA.Iws.Core.Cqrs
{
    using System.Threading.Tasks;

    public interface ICommandBus
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
