using Estore.Core.Entities;
using Estore.Data.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Estore.Data.Concrete
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new() //Repository classına gönderilecek T nin şartları: T(brand, category, product vb) bir class olmalı, IEntity den implemente almalı ve new lenebilir olmalı 
    {
        internal DatabaseContext _context; //Boş bir database context oluşturduk
        internal DbSet<T> _dbSet; // Boş bir db set tanımladık. Repository e gönderilecek T classını parametreden verdik

        public Repository(DatabaseContext context)
        {
            _context = context; //_context i burada doldurkuk, aşağıda kullanabilmek için(doldurmadan kullanmak istersek null reference hatası olur)
            _dbSet = context.Set<T>(); // Repository e gönderilecek T classı için context üzerindeki db sete göre kendini ayarla
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public T Find(int id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T> FindAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return _dbSet.FirstOrDefault(expression);
        }

        public List<T> GetAll()
        {
            return _dbSet.AsNoTracking().ToList(); //Eğer sadece listeleme yapacaksak yani kayıt güncelleme gibi bir işlem yapmayacaksak Entity Frameworkteki AsNoTracking yöntemi ile listeyi daha performanslı çekebiliriz
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression)
        {
            return _dbSet.AsNoTracking().Where(expression).ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.AsNoTracking().Where(expression).ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }

        public int Save()
        {
            return _context.SaveChanges(); //Entity framework de SaveChanges(ekle, güncelle, sil vb işlemleri db ye işeleyen metot) metodu direkt context üzerinden çalışır, dbset in böyle bir metodu olmadığı için  _context.SaveChanges() diyerek database conteximiz üzerindeki tüm işlemleri(ekleme, güncelleme, sil vb) veritabanına yansıtmamızı sağlayan bu metodu 1 kere çağırmamız gerekli yoksa işlemler dbye işlenmez!
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
