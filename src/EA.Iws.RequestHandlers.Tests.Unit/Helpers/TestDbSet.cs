namespace EA.Iws.RequestHandlers.Tests.Unit.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Linq.Expressions;

    public class TestDbSet<TEntity> : DbSet<TEntity>, IQueryable, IEnumerable<TEntity>, IDbAsyncEnumerable<TEntity>
        where TEntity : class
    {
        private readonly ObservableCollection<TEntity> data;
        private readonly IQueryable query;

        public TestDbSet()
        {
            data = new ObservableCollection<TEntity>();
            query = data.AsQueryable();
        }

        public TestDbSet(List<TEntity> entities)
        {
            data = new ObservableCollection<TEntity>(entities);
            query = data.AsQueryable();
        }

        public override ObservableCollection<TEntity> Local
        {
            get { return data; }
        }

        IDbAsyncEnumerator<TEntity> IDbAsyncEnumerable<TEntity>.GetAsyncEnumerator()
        {
            return new TestDbAsyncEnumerator<TEntity>(data.GetEnumerator());
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        Type IQueryable.ElementType
        {
            get { return query.ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return new TestDbAsyncQueryProvider<TEntity>(query.Provider); }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }

        public override TEntity Add(TEntity item)
        {
            data.Add(item);
            return item;
        }

        public override IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            var addRange = entities as TEntity[] ?? entities.ToArray();
            foreach (var entity in addRange)
            {
                data.Add(entity);
            }
            return addRange;
        }

        public override TEntity Remove(TEntity item)
        {
            data.Remove(item);
            return item;
        }

        public override IEnumerable<TEntity> RemoveRange(IEnumerable<TEntity> entities)
        {
            var removeRange = entities as TEntity[] ?? entities.ToArray();
            foreach (var entity in removeRange)
            {
                data.Remove(entity);
            }
            return removeRange;
        }

        public override TEntity Attach(TEntity item)
        {
            data.Add(item);
            return item;
        }

        public override TEntity Create()
        {
            return Activator.CreateInstance<TEntity>();
        }

        public override TDerivedEntity Create<TDerivedEntity>()
        {
            return Activator.CreateInstance<TDerivedEntity>();
        }
    }
}