using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvc.ApplicationCore.Interfaces
{
    public interface IRepository<T>
    {
        void Create(T item);

        void Remove(int id);

        IEnumerable<T> GetAll();

        IEnumerable<T> Where(Func<T, bool> predicate);
    }
}
