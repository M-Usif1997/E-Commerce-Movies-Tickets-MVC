using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace E_Commerce_Movies.Data.Base
{
    public class EntityBaseRepository<T> : IEntityBaseRepository<T> where T : class, IEntityBase, new()
    {
        private readonly AppDbContext _context;
        private DbSet<T> Entities;
        public EntityBaseRepository(AppDbContext context)
        {
            _context = context;
            Entities = _context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
            await Entities.AddAsync(entity);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(int id)
        {
            T existing = await GetByIdAsync(id);
            _context.Entry<T>(existing).State = EntityState.Deleted;
            Entities.Remove(existing);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await Entities.ToListAsync();

        public async Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = Entities;
            query = includeProperties.Aggregate(query, (current, includeProperties) => current.Include(includeProperties));   // aggregate (TAccumlate seed parameter data Entity  , so current entity is seed to include other entities
            return await query.ToListAsync();
            
        }

        public async Task<T> GetByIdAsync(int id) => await Entities.FirstOrDefaultAsync(n => n.Id == id);


        public async Task UpdateAsync(int id, T entity)
        {
            Entities.Attach(entity);  //calling Attach for an entity that is currently in the Added state will change its state to Unchanged.
            _context.Entry<T>(entity).State = EntityState.Modified;    //When you change the state to Modified all the properties of the entity will be marked as modified and all the property values will be sent to the database when SaveChanges is called.
            await _context.SaveChangesAsync();
        }
    }
}
