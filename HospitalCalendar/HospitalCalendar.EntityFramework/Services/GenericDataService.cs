using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using HospitalCalendar.Domain.Services;
using HospitalCalendar.Domain.Models;
using HospitalCalendar.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            using(HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
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
                T entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.ID == id);

                return entity;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            using (HospitalCalendarDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<T> entities = await context.Set<T>().ToListAsync();

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
