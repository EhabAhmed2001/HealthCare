using HealthCare.Core.Entities.Data;
using HealthCare.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        Task<int> CompleteAsync();
        IGenericRepository<T> CreateRepository<T>() where T : AppEntity;
    }
}
