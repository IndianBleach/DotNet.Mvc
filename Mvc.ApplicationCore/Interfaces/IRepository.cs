using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Interfaces
{
    public interface IRepository<T>
    {

        T FirstWhere(Func<T, bool> predicate);
        void Create(T model);
        ICollection<T> GetAll();
        ICollection<T> Where(Func<T, bool> predicate);
        void Update(T model);
        void Remove(int id);
        T Find(int id);
        void SaveChanges();
    }
}
