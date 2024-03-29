using HealthCare.Core;
using HealthCare.Core.Entities.Data;
using HealthCare.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repository
{
    public static class SpecificationEvaluator <T> where T : AppEntity
    {
        public static async Task<IQueryable<T>> GetQuery(IQueryable<T> DbContext, ISpecifications<T> Spec)
        {
            var Query = DbContext;

            if(Spec.Criteria is not null)
                Query = Query.Where(Spec.Criteria);

            if (Spec.CountEnable)
                Spec.Count = await Query.CountAsync();

            if (Spec.OrderBy is not null)
                Query = Query.OrderBy(Spec.OrderBy);

            else if (Spec.OrderByDesc is not null)
                Query = Query.OrderByDescending(Spec.OrderByDesc);

            if (Spec.IsPaginationEnable)
            {
                Query = Query.Skip(Spec.Skip).Take(Spec.Take);
            }

            if (Spec.Includes is not null)
                Query = Spec.Includes.Aggregate(Query, (currentquery, includeexpression) => currentquery.Include(includeexpression));

            return Query;
        }
    }
}
