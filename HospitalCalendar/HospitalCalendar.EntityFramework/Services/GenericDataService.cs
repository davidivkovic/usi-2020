using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalCalendar.EntityFramework.Services
{
    public class GenericDataService<T> : IDataService<T> where T : DomainObject
    {
        protected readonly HospitalCalendarDbContextFactory _contextFactory;

        public GenericDataService(HospitalCalendarDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<T> Create(T entity)
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<T> createdResult = await context.Set<T>().AddAsync(entity);
                await context.SaveChangesAsync();

                return createdResult.Entity;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.ID == id);
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<T> Get(Guid id)
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.ID == id && e.IsActive);

                return entity;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<T> entities = await context.Set<T>().Where(e => e.IsActive).ToListAsync();

                return entities;
            }
        }

        public async Task<T> Update(T entity)
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                context.Set<T>().Update(entity);
                await context.SaveChangesAsync();

                return entity;
            }
        }
    }
}
