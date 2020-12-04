namespace EA.Iws.DataAccess.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class StaticDataCachingRepositoryBase<T>
    {
        private static IEnumerable<T> cache;
        private static readonly object LockObject = new object();
        protected readonly IwsContext Context;

        public StaticDataCachingRepositoryBase(IwsContext context)
        {
            this.Context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await RetreiveFromCache();
        }

        /// <summary>
        /// Blocking call
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {
            if (cache == null)
            {
                LoadCache();
            }
            return cache;
        }

        protected abstract T[] GetFromContext();
        
        protected async Task<IEnumerable<T>> RetreiveFromCache()
        {
            if (cache == null)
            {
                await Task.Run(() => LoadCache());
            }
            return cache;
        }

        private void LoadCache()
        {
            lock (LockObject)
            {
                if (cache == null)
                {
                    cache = GetFromContext();
                }
            }
        }
    }
}
