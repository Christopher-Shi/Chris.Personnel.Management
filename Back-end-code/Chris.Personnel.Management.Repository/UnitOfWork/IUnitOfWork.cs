using System;
using System.Threading.Tasks;

namespace Chris.Personnel.Management.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit();
    }
}
