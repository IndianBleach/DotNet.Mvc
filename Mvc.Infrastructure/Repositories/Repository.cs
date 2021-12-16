using Mvc.ApplicationCore.Entities;
using Mvc.ApplicationCore.Interfaces;
using Mvc.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> 
        where T: class
    {
        private readonly ApplicationContext _dbContext;

        public Repository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T FirstWhere(Func<T, bool> predicate)
        {
            return _dbContext.Set<T>().FirstOrDefault(predicate);
        }

        public void Create(T model)
        {
            _dbContext.Set<T>().Add(model);
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public T Find(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

        public void Update(T model)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> Where(Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
