namespace EA.Iws.Core.Cqrs
{
    using System.Threading.Tasks;

    public interface IQueryBus
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}
