using HealthCare.Core;
using HealthCare.Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Repository
{
    public static class SpecificationEvaluator <T> where T : BaseEntity
    {
        public static async Task<IQueryable<T>> GetQuery(IQueryable<T> Context, ISpecifications<T> Spec)
        {
            var Query = Context;

            if(Spec.Criteria is not null)
                Query = Query.Where(Spec.Criteria);

            if (Spec.OrderBy is not null)
                Query = Query.OrderBy(Spec.OrderBy);

            else if (Spec.OrderByDesc is not null)
                Query = Query.OrderByDescending(Spec.OrderByDesc);

            if (Spec.Includes is not null)
                Query = Spec.Includes.Aggregate(Query, (currentquery, includeexpression) => currentquery.Include(includeexpression));

            return Query;
        }
    }
}
