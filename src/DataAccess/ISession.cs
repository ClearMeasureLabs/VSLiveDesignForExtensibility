using System;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public interface ISession : NHibernate.ISession, IDisposable
    {
        
    }
}