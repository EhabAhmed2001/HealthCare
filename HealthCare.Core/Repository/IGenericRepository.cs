using HealthCare.Core.Entities.Data;
using HealthCare.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Repository
{
    public interface IGenericRepository <T> where T : AppEntity
    {
        #region Without Specification

        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);

        #endregion


        #region With Specification

        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);
        Task<T?> GetByIdWithSpecAsync(ISpecifications<T> Spec);

        #endregion
    }
}
