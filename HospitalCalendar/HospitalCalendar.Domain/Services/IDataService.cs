using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HospitalCalendar.Domain.Services
{
    public interface IDataService<T>
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> Get(Guid ID);

        Task<T> Create(T entity);

        Task<T> Update(T entity);

        Task<bool> Delete(Guid ID);
    }
}
