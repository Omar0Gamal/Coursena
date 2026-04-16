using Coursna.Core.Domain.RepositoryInterface;
using Coursna.Infrastrcuter.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursna.Infrastrcuter.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet=_context.Set<T>();
        }
        public async Task AddAsync(T entity)
        {
           await _dbSet.AddAsync(entity);
        }

        public  void DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
          return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
           return await _dbSet.FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public  void UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
