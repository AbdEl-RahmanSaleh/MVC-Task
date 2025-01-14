using Core.Context;
using Core.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public  T GetById(int? id)
        {
            if (typeof(T) == typeof(Order))
                return _context.Set<Order>().Include(x =>x.OrderItems).FirstOrDefault(x => x.Id == id) as T;
            return _context.Set<T>().FirstOrDefault(x => x.Id == id);

        }

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
            var x =  _context.Set<T>().FirstOrDefault(d => d.Id == id);
            x.IsDeleted = true;
            _context.SaveChanges();
        }

    }
}
