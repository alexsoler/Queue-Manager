using AplicationCore.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AplicationCore.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(params object[] keyValues);
        T GetSingleBySpec(ISpecification<T> spec);
        IEnumerable<T> ListAll();
        IEnumerable<T> List(ISpecification<T> spec);
        T Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        bool Exist(ISpecification<T> spec);
        void Delete(T entity);
        int Count(ISpecification<T> spec);
    }
}
