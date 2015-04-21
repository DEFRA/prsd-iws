namespace EA.Iws.Core.Cqrs
{
    using System.Threading.Tasks;

    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> ExecuteAsync(TQuery query);
    }
}
