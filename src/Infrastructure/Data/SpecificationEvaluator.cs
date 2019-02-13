using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.QueueManager.Infrastructure.Data
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var query = inputQuery;

            //Modifica el IQueryable usando un criterio si este existe
            if (specification.Criterio != null)
            {
                query = query.Where(specification.Criterio);
            }

            //Se establece el include por medio de una expresion lambda
            query = specification.Includes.Aggregate(query,
                                    (current, include) => current.Include(include));

            //Se establece el include por medio de un string
            query = specification.IncludesStrings.Aggregate(query,
                                    (current, include) => current.Include(include));

            //Se ordenan los registros ascendentemente o descendentemente si así se especifica 
            if (specification.OrderBy != null)
            {
                query = query.OrderBy(specification.OrderBy);
            }
            else if(specification.OrderByDescending != null)
            {
                query = query.OrderByDescending(specification.OrderByDescending);
            }

            //Si se activa la paginación se devuelven los registros tal como se especifico
            if(specification.isPagingEnabled)
            {
                query = query.Skip(specification.Skip)
                    .Take(specification.Take);
            }

            return query;
        }
    }
}
