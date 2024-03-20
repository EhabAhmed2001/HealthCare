using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Specifications
{
    public class Specifications<T> :ISpecifications<T> where T : BaseEntity
    {
        // Sign For Property For Where [Where(U => U.Id == id)]
        public Expression<Func<T, bool>> Criteria { get; set; }

        // Sign For Property For Include [Include(U => U.History)]
        public List<Expression<Func<T, object>>> Includes { get; set; }

        // Property for Order by Asc
        public Expression<Func<T, object>> OrderBy { get; set; }

        // Property For Order By Desc
        public Expression<Func<T, object>> OrderByDesc { get; set; }

        // For Pagination
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnable { get; set; } = true;

        // To Claculate The Count Of Response Items
        public bool CountEnable { get; set; } = true;
        public int Count { get; set; }

        //Get All
        public Specifications()
        {

        }

        // Get By Criteria
        public Specifications(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
        }

        // Apply Pagination
        public void ApplyPagination(int skip, int take)
        {
            Skip = skip;
            Take = take;
        }


    }
}
