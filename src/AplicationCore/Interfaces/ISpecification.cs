using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ApplicationCore.Interfaces
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criterio { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludesStrings { get; }
        Expression<Func<T, object>> OrderBy { get; }
        Expression<Func<T, object>> OrderByDescending { get; }

        int Take { get; }
        int Skip { get; }
        bool isPagingEnabled { get; }
        bool ignoreQueryFilter { get; }
    }
}
