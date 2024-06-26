﻿using HealthCare.Core.Entities.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Core.Specifications
{
    public interface ISpecifications <T> where T : AppEntity
    {
        public Expression<Func<T,bool>> Criteria { get; set; } 
        public List<Expression<Func<T,object>>> Includes { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnable { get; set; }
        public bool CountEnable { get; set; }
        public int Count { get; set; }

    }
}
