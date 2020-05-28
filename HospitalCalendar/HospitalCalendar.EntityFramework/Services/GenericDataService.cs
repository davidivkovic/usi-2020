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
        protected readonly HospitalCalendarDbContext Context;

        public GenericDataService(HospitalCalendarDbContextFactory contextFactory)
        {
            ContextFactory = contextFactory;
            Context = ContextFactory.CreateDbContext();
        }

        public async Task<T> Create(T entity)
        {
            var createdResult = await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return createdResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            var entity = await Context.Set<T>().FirstOrDefaultAsync((e) => e.ID == id);
            Context.Set<T>().Remove(entity);
            await Context.SaveChangesAsync();
            return true;
        }

        public async Task<T> Get(Guid id)
        {
            var entity = await Context.Set<T>().FirstOrDefaultAsync(e => e.ID == id && e.IsActive);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            IEnumerable<T> entities = await Context.Set<T>().Where(e => e.IsActive).ToListAsync();
            return entities;
        }

        public async Task<T> Update(T entity)
        {
            Context.Set<T>().Update(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
    }
}
