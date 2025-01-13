using Core.Context;
using Core.Entities;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class GenericRepo<T> : IGenericRepo<T> where T : BaseEntity
    {

        private readonly AppDbContext _context;

        public GenericRepo(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<T> GetAll()
            => _context.Set<T>().ToList();

        public T GetById(int? id)
         => _context.Set<T>().FirstOrDefault(x => x.Id == id);

        public int Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return _context.SaveChanges();   
        }
        public int Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return _context.SaveChanges();
        }

        public void Delete(int? id)
        {
            var product = _context.Products.FirstOrDefault(d => d.Id == id);
            product.IsDeleted = true;
            _context.SaveChanges();
        }

    }
}
