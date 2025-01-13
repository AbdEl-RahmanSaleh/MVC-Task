using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IGenericRepo<T> where T : BaseEntity
    {
        T GetById(int? id);
        IEnumerable<T> GetAll();
        int Add(T entity);
        int Update(T entity);
        void Delete(int? id);
    }
}
