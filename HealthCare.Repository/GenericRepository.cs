using HealthCare.Core;
using HealthCare.Core.Repository;
using HealthCare.Core.Specifications;
using HealthCare.Repository.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly HealthCareContext _dbContext;

        public GenericRepository(HealthCareContext DbContext)
        {
            _dbContext = DbContext;
        }

        #region Without Specification

        public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbContext.Set<T>().ToListAsync();
        public async Task<T?> GetByIdAsync(int id)
        => await _dbContext.Set<T>().FindAsync(id);

        #endregion


        #region With Specification

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec)
        => await GenerateSpec(Spec).ToListAsync();
        public async Task<T?> GetByIdWithSpecAsync(ISpecifications<T> Spec)
         => await GenerateSpec(Spec).FirstOrDefaultAsync();


        private IQueryable<T> GenerateSpec(ISpecifications<T> Spec)
        => SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec).Result; 

        #endregion

    }
}
