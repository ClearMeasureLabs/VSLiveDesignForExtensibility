using System;
using System.Data;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Stat;
using NHibernate.Type;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class DbContextWrapper : IDbContext
    {
        private NHibernate.ISession _sessionImplementation;

        public DbContextWrapper(NHibernate.ISession sessionImplementation)
        {
            _sessionImplementation = sessionImplementation;
        }

        public void Dispose()
        {
            _sessionImplementation.Dispose();
        }

        public void Flush()
        {
            _sessionImplementation.Flush();
        }

        public IDbConnection Disconnect()
        {
            return _sessionImplementation.Disconnect();
        }

        public void Reconnect()
        {
            _sessionImplementation.Reconnect();
        }

        public void Reconnect(IDbConnection connection)
        {
            _sessionImplementation.Reconnect(connection);
        }

        public IDbConnection Close()
        {
            return _sessionImplementation.Close();
        }

        public void CancelQuery()
        {
            _sessionImplementation.CancelQuery();
        }

        public bool IsDirty()
        {
            return _sessionImplementation.IsDirty();
        }

        public bool IsReadOnly(object entityOrProxy)
        {
            return _sessionImplementation.IsReadOnly(entityOrProxy);
        }

        public void SetReadOnly(object entityOrProxy, bool readOnly)
        {
            _sessionImplementation.SetReadOnly(entityOrProxy, readOnly);
        }

        public object GetIdentifier(object obj)
        {
            return _sessionImplementation.GetIdentifier(obj);
        }

        public bool Contains(object obj)
        {
            return _sessionImplementation.Contains(obj);
        }

        public void Evict(object obj)
        {
            _sessionImplementation.Evict(obj);
        }

        public object Load(Type theType, object id, LockMode lockMode)
        {
            return _sessionImplementation.Load(theType, id, lockMode);
        }

        public object Load(string entityName, object id, LockMode lockMode)
        {
            return _sessionImplementation.Load(entityName, id, lockMode);
        }

        public object Load(Type theType, object id)
        {
            return _sessionImplementation.Load(theType, id);
        }

        public T Load<T>(object id, LockMode lockMode)
        {
            return _sessionImplementation.Load<T>(id, lockMode);
        }

        public T Load<T>(object id)
        {
            return _sessionImplementation.Load<T>(id);
        }

        public object Load(string entityName, object id)
        {
            return _sessionImplementation.Load(entityName, id);
        }

        public void Load(object obj, object id)
        {
            _sessionImplementation.Load(obj, id);
        }

        public void Replicate(object obj, ReplicationMode replicationMode)
        {
            _sessionImplementation.Replicate(obj, replicationMode);
        }

        public void Replicate(string entityName, object obj, ReplicationMode replicationMode)
        {
            _sessionImplementation.Replicate(entityName, obj, replicationMode);
        }

        public object Save(object obj)
        {
            return _sessionImplementation.Save(obj);
        }

        public void Save(object obj, object id)
        {
            _sessionImplementation.Save(obj, id);
        }

        public object Save(string entityName, object obj)
        {
            return _sessionImplementation.Save(entityName, obj);
        }

        public void SaveOrUpdate(object obj)
        {
            _sessionImplementation.SaveOrUpdate(obj);
        }

        public void SaveOrUpdate(string entityName, object obj)
        {
            _sessionImplementation.SaveOrUpdate(entityName, obj);
        }

        public void Update(object obj)
        {
            _sessionImplementation.Update(obj);
        }

        public void Update(object obj, object id)
        {
            _sessionImplementation.Update(obj, id);
        }

        public void Update(string entityName, object obj)
        {
            _sessionImplementation.Update(entityName, obj);
        }

        public object Merge(object obj)
        {
            return _sessionImplementation.Merge(obj);
        }

        public object Merge(string entityName, object obj)
        {
            return _sessionImplementation.Merge(entityName, obj);
        }

        public T Merge<T>(T entity) where T : class
        {
            return _sessionImplementation.Merge(entity);
        }

        public T Merge<T>(string entityName, T entity) where T : class
        {
            return _sessionImplementation.Merge(entityName, entity);
        }

        public void Persist(object obj)
        {
            _sessionImplementation.Persist(obj);
        }

        public void Persist(string entityName, object obj)
        {
            _sessionImplementation.Persist(entityName, obj);
        }

        public object SaveOrUpdateCopy(object obj)
        {
            return _sessionImplementation.SaveOrUpdateCopy(obj);
        }

        public object SaveOrUpdateCopy(object obj, object id)
        {
            return _sessionImplementation.SaveOrUpdateCopy(obj, id);
        }

        public void Delete(object obj)
        {
            _sessionImplementation.Delete(obj);
        }

        public void Delete(string entityName, object obj)
        {
            _sessionImplementation.Delete(entityName, obj);
        }

        public int Delete(string query)
        {
            return _sessionImplementation.Delete(query);
        }

        public int Delete(string query, object value, IType type)
        {
            return _sessionImplementation.Delete(query, value, type);
        }

        public int Delete(string query, object[] values, IType[] types)
        {
            return _sessionImplementation.Delete(query, values, types);
        }

        public void Lock(object obj, LockMode lockMode)
        {
            _sessionImplementation.Lock(obj, lockMode);
        }

        public void Lock(string entityName, object obj, LockMode lockMode)
        {
            _sessionImplementation.Lock(entityName, obj, lockMode);
        }

        public void Refresh(object obj)
        {
            _sessionImplementation.Refresh(obj);
        }

        public void Refresh(object obj, LockMode lockMode)
        {
            _sessionImplementation.Refresh(obj, lockMode);
        }

        public LockMode GetCurrentLockMode(object obj)
        {
            return _sessionImplementation.GetCurrentLockMode(obj);
        }

        public ITransaction BeginTransaction()
        {
            return _sessionImplementation.BeginTransaction();
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return _sessionImplementation.BeginTransaction(isolationLevel);
        }

        public ICriteria CreateCriteria<T>() where T : class
        {
            return _sessionImplementation.CreateCriteria<T>();
        }

        public ICriteria CreateCriteria<T>(string alias) where T : class
        {
            return _sessionImplementation.CreateCriteria<T>(alias);
        }

        public ICriteria CreateCriteria(Type persistentClass)
        {
            return _sessionImplementation.CreateCriteria(persistentClass);
        }

        public ICriteria CreateCriteria(Type persistentClass, string alias)
        {
            return _sessionImplementation.CreateCriteria(persistentClass, alias);
        }

        public ICriteria CreateCriteria(string entityName)
        {
            return _sessionImplementation.CreateCriteria(entityName);
        }

        public ICriteria CreateCriteria(string entityName, string alias)
        {
            return _sessionImplementation.CreateCriteria(entityName, alias);
        }

        public IQueryOver<T, T> QueryOver<T>() where T : class
        {
            return _sessionImplementation.QueryOver<T>();
        }

        public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class
        {
            return _sessionImplementation.QueryOver(alias);
        }

        public IQueryOver<T, T> QueryOver<T>(string entityName) where T : class
        {
            return _sessionImplementation.QueryOver<T>(entityName);
        }

        public IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class
        {
            return _sessionImplementation.QueryOver(entityName, alias);
        }

        public IQuery CreateQuery(string queryString)
        {
            return _sessionImplementation.CreateQuery(queryString);
        }

        public IQuery CreateFilter(object collection, string queryString)
        {
            return _sessionImplementation.CreateFilter(collection, queryString);
        }

        public IQuery GetNamedQuery(string queryName)
        {
            return _sessionImplementation.GetNamedQuery(queryName);
        }

        public ISQLQuery CreateSQLQuery(string queryString)
        {
            return _sessionImplementation.CreateSQLQuery(queryString);
        }

        public void Clear()
        {
            _sessionImplementation.Clear();
        }

        public object Get(Type clazz, object id)
        {
            return _sessionImplementation.Get(clazz, id);
        }

        public object Get(Type clazz, object id, LockMode lockMode)
        {
            return _sessionImplementation.Get(clazz, id, lockMode);
        }

        public object Get(string entityName, object id)
        {
            return _sessionImplementation.Get(entityName, id);
        }

        public T Get<T>(object id)
        {
            return _sessionImplementation.Get<T>(id);
        }

        public T Get<T>(object id, LockMode lockMode)
        {
            return _sessionImplementation.Get<T>(id, lockMode);
        }

        public string GetEntityName(object obj)
        {
            return _sessionImplementation.GetEntityName(obj);
        }

        public IFilter EnableFilter(string filterName)
        {
            return _sessionImplementation.EnableFilter(filterName);
        }

        public IFilter GetEnabledFilter(string filterName)
        {
            return _sessionImplementation.GetEnabledFilter(filterName);
        }

        public void DisableFilter(string filterName)
        {
            _sessionImplementation.DisableFilter(filterName);
        }

        public IMultiQuery CreateMultiQuery()
        {
            return _sessionImplementation.CreateMultiQuery();
        }

        public NHibernate.ISession SetBatchSize(int batchSize)
        {
            return _sessionImplementation.SetBatchSize(batchSize);
        }

        public ISessionImplementor GetSessionImplementation()
        {
            return _sessionImplementation.GetSessionImplementation();
        }

        public IMultiCriteria CreateMultiCriteria()
        {
            return _sessionImplementation.CreateMultiCriteria();
        }

        public NHibernate.ISession GetSession(EntityMode entityMode)
        {
            return _sessionImplementation.GetSession(entityMode);
        }

        public EntityMode ActiveEntityMode
        {
            get { return _sessionImplementation.ActiveEntityMode; }
        }

        public FlushMode FlushMode
        {
            get { return _sessionImplementation.FlushMode; }
            set { _sessionImplementation.FlushMode = value; }
        }

        public CacheMode CacheMode
        {
            get { return _sessionImplementation.CacheMode; }
            set { _sessionImplementation.CacheMode = value; }
        }

        public ISessionFactory SessionFactory
        {
            get { return _sessionImplementation.SessionFactory; }
        }

        public IDbConnection Connection
        {
            get { return _sessionImplementation.Connection; }
        }

        public bool IsOpen
        {
            get { return _sessionImplementation.IsOpen; }
        }

        public bool IsConnected
        {
            get { return _sessionImplementation.IsConnected; }
        }

        public bool DefaultReadOnly
        {
            get { return _sessionImplementation.DefaultReadOnly; }
            set { _sessionImplementation.DefaultReadOnly = value; }
        }

        public ITransaction Transaction
        {
            get { return _sessionImplementation.Transaction; }
        }

        public ISessionStatistics Statistics
        {
            get { return _sessionImplementation.Statistics; }
        }
    }
}