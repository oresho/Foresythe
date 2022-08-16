using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Foresythe.Configurations;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Foresythe.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext _repositoryContext;

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public async Task CreateAsync(T entity)
        {
            await _repositoryContext.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _repositoryContext.Set<T>().Remove(entity);
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return _repositoryContext.Set<T>().Where(expression);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repositoryContext.Set<T>().ToListAsync();
        }

        public async Task SaveAsync()
        {
            await _repositoryContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _repositoryContext.Set<T>().Update(entity);
        }
    }
}
