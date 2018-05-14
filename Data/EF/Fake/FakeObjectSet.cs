using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace Data.EF.Fake
{
    public class FakeObjectSet<TEntity> : IObjectSet<TEntity> where TEntity : class
    {

        HashSet<TEntity> _data;
        IQueryable _query;

        public FakeObjectSet() : this(new List<TEntity>()) { }

        public FakeObjectSet(IEnumerable<TEntity> testData)
        {
            _data = new HashSet<TEntity>(testData);
            _query = _data.AsQueryable();
        }

        public void AddObject(TEntity item)
        {
            _data.Add(item);
        }

        public void DeleteObject(TEntity item)
        {
            _data.Remove(item);
        }

        public void Detach(TEntity entity)
        {
            _data.Remove(entity);
        }

        public void Attach(TEntity item)
        {
            _data.Add(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        Type IQueryable.ElementType
        {
            get { return _query.ElementType; }
        }

        Expression IQueryable.Expression
        {
            get { return _query.Expression; }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _query.Provider; }
        }
    }
}
