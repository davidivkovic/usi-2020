using HospitalCalendar.Domain.Models;
using HospitalCalendar.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HospitalCalendar.EntityFramework.Services
{
    public class GenericDataService<T> : IDataService<T> where T : DomainObject
    {
        protected readonly HospitalCalendarDbContextFactory ContextFactory;
       // protected readonly HospitalCalendarDbContext Context;

        public GenericDataService(HospitalCalendarDbContextFactory contextFactory)
        {
            ContextFactory = contextFactory;
           // Context = ContextFactory.CreateDbContext();
        }

        public async Task<T> Create(T entity)
        {
            await using var context = ContextFactory.CreateDbContext();
            var createdResult = await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
            return createdResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            await using var context = ContextFactory.CreateDbContext();
            var entity = await context.Set<T>().FirstOrDefaultAsync((e) => e.ID == id);
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<T> Get(Guid id)
        {
            await using var context = ContextFactory.CreateDbContext();
            var entity = await context.Set<T>().FirstOrDefaultAsync(e => e.ID == id && e.IsActive);
            return entity;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            await using var context = ContextFactory.CreateDbContext();
            IEnumerable<T> entities = await context.Set<T>().Where(e => e.IsActive).ToListAsync();
            return entities;
        }

        public async Task<T> Update(T entity)
        {
            await using var context = ContextFactory.CreateDbContext();
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
            return await context.Set<T>().FindAsync(entity.ID);
        }

        public async Task<T> Update(T entity, params Expression<Func<T, object>>[] navigation)
        {
            await using var context = ContextFactory.CreateDbContext();
            var dbEntity = await context.FindAsync<T>(entity.ID);

            var dbEntry = context.Entry(dbEntity);
            dbEntry.CurrentValues.SetValues(entity);

            foreach (var property in navigation)
            {
                var propertyName = property.GetPropertyAccess().Name;
                var dbItemsEntry = dbEntry.Collection(propertyName);
                var accessor = dbItemsEntry.Metadata.GetCollectionAccessor();

                await dbItemsEntry.LoadAsync();
                var dbItemsMap = ((IEnumerable<DomainObject>)dbItemsEntry.CurrentValue)
                    .ToDictionary(e => e.ID);

                var items = (IEnumerable<DomainObject>)accessor.GetOrCreate(entity, false);

                foreach (var item in items)
                {
                    if (!dbItemsMap.TryGetValue(item.ID, out var oldItem))
                        accessor.Add(dbEntity, item, false);
                    else
                    {
                        context.Entry(oldItem).CurrentValues.SetValues(item);
                        dbItemsMap.Remove(item.ID);
                    }
                }

                foreach (var oldItem in dbItemsMap.Values)
                    accessor.Remove(dbEntity, oldItem);
            }

            await context.SaveChangesAsync();
            return await context.Set<T>().FindAsync(entity.ID);
        }
    }
}
