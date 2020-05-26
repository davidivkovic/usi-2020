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
        protected readonly HospitalCalendarDbContextFactory ContextFactory;
        protected readonly HospitalCalendarDbContext _context;

        public GenericDataService(HospitalCalendarDbContextFactory contextFactory)
        {
            ContextFactory = contextFactory;
            _context = ContextFactory.CreateDbContext();
        }

        public async Task<T> Create(T entity)
        {

                EntityEntry<T> createdResult = await _context.Set<T>().AddAsync(entity);
                await _context.SaveChangesAsync();

                return createdResult.Entity;
            
        }

        public async Task<bool> Delete(Guid id)
        {

                T entity = await _context.Set<T>().FirstOrDefaultAsync((e) => e.ID == id);
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();

                return true;

        }

        public async Task<T> Get(Guid id)
        {

                T entity = await _context.Set<T>().FirstOrDefaultAsync(e => e.ID == id && e.IsActive);

                return entity;

        }

        public async Task<IEnumerable<T>> GetAll()
        {
            IEnumerable<T> entities = await _context.Set<T>().Where(e => e.IsActive).ToListAsync();

            return entities;
        }

        public async Task<T> Update(T entity)
        {

                //context.Entry(entity).State = EntityState.Modified;
                _context.Set<T>().Update(entity);
                //context.Entry(await context.Set<T>().FirstOrDefaultAsync(T => T.ID == entity.ID)).CurrentValues.SetValues(entity); 
                await _context.SaveChangesAsync();

                return entity;

        }
    }
}
