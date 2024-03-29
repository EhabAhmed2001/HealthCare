using HealthCare.Core;
using HealthCare.Core.Entities.Data;
using HealthCare.Core.Repository;
using HealthCare.Repository.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HealthCareContext _dbContext;
        private Hashtable _Repos { get; set; } // To Save All Objects That Created To Reuse It Instead of Create It Again

        public UnitOfWork(HealthCareContext DbContext)
        {
            _dbContext = DbContext;
            _Repos = new Hashtable();
        }
        
        // To Save All Transaction To DataBase Through DbContext
        public async Task<int> CompleteAsync()
        => await _dbContext.SaveChangesAsync();


        // To Remove All Objects After Finishing Requist
        public ValueTask DisposeAsync()
        => _dbContext.DisposeAsync();

        // Create Object Of GenericRepository Instead of Create All Of These in GenericRepository
        // And Create Objects I Didn't Need
        public IGenericRepository<T> CreateRepository<T>() where T : AppEntity
        {
            var key = typeof(T).Name;
            if (!_Repos.ContainsKey(key))
            {
                var Repo = new GenericRepository<T>(_dbContext);
                _Repos.Add(key, Repo);
            }
            return (IGenericRepository<T>) _Repos[key]!;
        }
    }
}
