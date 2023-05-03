﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions; //kendi lambda Expression(x => x.) kullanabileceğimiz metotları yazmamızı sağlayan kütüphane

namespace Estore.Data.Abstract
{
    public interface IRepository<T> where T : class //IRepository interface i dışarıdan alacağı T tipi bir parametreyle çalışacak ve where şartı ile bu T nin veri tipi bir class olmalıdır.
    {
        //Senkron Metotlar
        List<T> GetAll(); //db deki tüm kayıtları çekmemizi sağlayacak metot imzası
        List<T> GetAll(Expression<Func<T, bool>> expression); // expression Uygulamada verileri listelerken  p=>p.IsActive vb gibi sorgulama ve filtreleme kodları kullanabilmemizi sağlar
        T Get(Expression<Func<T, bool>> expression);
        T Find(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        int Save();
        //Asenkron Metotlar
        Task<T> FindAsync(int id);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> expression);
        Task AddAsync(T entity);
        Task<int> SaveAsync(); //Asenkron kaydetme

    }
}
