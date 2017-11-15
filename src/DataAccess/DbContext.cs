using System;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public interface IDbContext : NHibernate.ISession, IDisposable
    {
        
    }
}