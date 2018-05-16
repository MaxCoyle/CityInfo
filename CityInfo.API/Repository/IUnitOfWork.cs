using System;
using System.Data.SqlClient;

namespace CityInfo.API.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUnitOfWork Begin();
        void Commit(Action<SqlConnection, SqlTransaction> doWork);
        void Rollback();
    }
}