namespace EA.Iws.Cqrs.Autofac
{
    using global::Autofac;
    using Core.Cqrs;
    using System.Threading.Tasks;

    public class AutofacQueryBus : IQueryBus
    {
        private readonly IComponentContext context;

        public AutofacQueryBus(IComponentContext context)
        {
            this.context = context;
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            // In order to resolve our query handler without a TQuery type parameter we need to use reflection.
            // This gets the type of the handler with the parameters: IQueryHandler<TQuery, TResult>.
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));

            // We next get the registered handler of the correct type defined above.
            object handler;
            if (context.TryResolve(handlerType, out handler))
            {
                var executeMethod = handlerType.GetMethod("ExecuteAsync");

                // Run the Execute method on the handler passing the query.
                var resultTask = (Task<TResult>)executeMethod.Invoke(handler, new object[] { query });

                var result = await resultTask;

                return result;
            }
            // No handler with this signature was registered on the container.
            var exceptionMessage =
                string.Format("No query handler registered for query type {0} with return type {1}",
                    query.GetType().Name,
                    typeof(TResult).Name);

            throw new MissingHandlerException(exceptionMessage);
        }
    }
}