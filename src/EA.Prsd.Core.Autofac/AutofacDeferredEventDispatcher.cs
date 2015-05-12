namespace EA.Prsd.Core.Autofac
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Domain;
    using global::Autofac;

    public class AutofacDeferredEventDispatcher : IDeferredEventDispatcher
    {
        private readonly IComponentContext context;
        private readonly ConcurrentQueue<IEvent> events;

        public AutofacDeferredEventDispatcher(IComponentContext context)
        {
            this.context = context;
            events = new ConcurrentQueue<IEvent>();
        }

        public Task Dispatch<TEvent>(TEvent e) where TEvent : IEvent
        {
            events.Enqueue(e);
            return Task.FromResult(0);
        }

        public async Task Resolve()
        {
            IEvent e;
            while (events.TryDequeue(out e))
            {
                await HandleEvent(e);
            }
        }

        private async Task HandleEvent(IEvent e)
        {
            var handlerType = typeof(IEventHandler<>).MakeGenericType(e.GetType());
            var collectionType = typeof(IEnumerable<>).MakeGenericType(handlerType);
            var handlers = ((IEnumerable<object>)context.Resolve(collectionType)).ToList();
            
            foreach (var handler in handlers)
            {
                MethodInfo handleMethod = handlerType.GetMethod("HandleAsync");

                var resultTask = (Task)handleMethod.Invoke(handler, new object[] { e });

                await resultTask;
            }
        }
    }
}