using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services
{
    public interface IDataService<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(Guid id);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(Guid id);
    }
}
