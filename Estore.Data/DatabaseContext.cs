using Estore.Core.Entities;
using Estore.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Estore.Data
{
    public class DatabaseContext : DbContext
    {
        // Katmanlı mimaride bir proje katmanından başka bir katmana erişebilmek için, bulunduğumuz Data projesinin Dependencies kısmına sağ tıklayıp > Add project references diyerek açılan pencereden Core projesine Tik atıp pencereyi kapatmamız gerekiyor
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Slider> Sliders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //OnConfiguring metodu EntityFrameworkCore ile gelir ve veritabanı bağlantı ayarlarını yapmamızı sağlar.
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB; Database=Estore; Trusted_Connection=True");
            //optionsBuilder.UseSqlServer(@"Server=CanlıServerAdı; Database=CanlıDatabase; Username=CanlıVeritabanıKullanıcıAdı; Password=CanlıVeritabanıŞifre");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //FluentAPI ile veritabanı tablolarımız oluşurken veri tiplerini db kurallarını burada tanımlayabiliriz.
            modelBuilder.Entity<AppUser>().Property(a => a.Name).IsRequired().HasMaxLength(50); //FluentAPI ile AppUser classının Name Property si için oluşacak veritabanı kolonu ayarlarını bu şekilde belirleyebiliriz.
            modelBuilder.Entity<AppUser>().Property(a => a.Surname).HasMaxLength(50);
            modelBuilder.Entity<AppUser>().Property(a => a.UserName).HasColumnType("varchar(50)").HasMaxLength(50);
            modelBuilder.Entity<AppUser>().Property(a => a.Password).IsRequired().HasColumnType("nvarchar(100)").HasMaxLength(100);
            modelBuilder.Entity<AppUser>().Property(a => a.Email).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<AppUser>().Property(a => a.Phone).HasMaxLength(20);

            //FluentAPI HasData ile db oluştuktan sonra başlangıç kayıtları ekleme

            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = 1,
                Email = "info@Estore.com",
                Password = "123",
                UserName = "Admin",
                IsActive = true,
                IsAdmin = true,
                Name = "Admin",
                UserGuid = Guid.NewGuid(), //Kullanıcıya benzersiz bir id no oluşturur.
            });

            // modelBuilder.ApplyConfiguration(new BrandConfigurations()); //Marka için yaptığımız konfigürasyon ayarlarını çağırdık.
            //modelBuilder.ApplyConfiguration(new CategoryConfigurations());
            //modelBuilder.ApplyConfiguration(new ContactConfigurations());
            //modelBuilder.ApplyConfiguration(new ProductConfigurations());
            //modelBuilder.ApplyConfiguration(new SliderConfigurations());
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); //Uygulamadaki tüm Configurations classlarını burada çalıştır.

            //Fluent Validation : data annotationdaki hata mesajları vb işlemlerini yönetebileceğimiz 3. parti paket.

            // Katmanlı mimaride MvcWebUI katmanından direkt data katmanına erişilmesi istenmez, arada bir iş katmanının tüm db süreçlerini yönetmesi istenir. Bu yüzden solutiona service katmanı ekleyip Mvc katmanından service katmanına erişim vermemiz gerekir. Service katmanı da data katmanına erişir. Data katmanı da core katmanına erişir, böylece MVCUI > Service > Data > Core ile en üstten en alt katmana kadar ulaşılabilmiş olunur.

            base.OnModelCreating(modelBuilder);
        }
    }
}