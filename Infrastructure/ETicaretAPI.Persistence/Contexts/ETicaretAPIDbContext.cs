using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Contexts
{
    public class ETicaretAPIDbContext : DbContext
    {
        public ETicaretAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// ETicaretAPI.Persistence.Repositories.WriteRepositor'deki SaveAsync kullanılan SaveChangesAsync metodu türünde override edilmiştir.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //ChangeTracker: Entity'ler üzerinden yapılan değişikliklerin ya da yeni eklenen verinin yakalanmasını sağlayan property. DbContext'ten gelir. Update operasyonlarında Track edilen verileri yakalayıp elde etmemizi sağlar.
            //Entries: Değişiklikleri izlenen entity'lerin listesini döner.
            var datas = ChangeTracker.Entries<BaseEntity>();

            foreach (var data in datas)
            {
                //_ = ifadesi switch kullandığımız için bir değişkene atamamızı istiyor. Memory'de allocate yapılmasını istemediğimiz için kullandık.
                _ = data.State switch
                {
                    EntityState.Added => data.Entity.CreatedDate = DateTime.UtcNow,
                    EntityState.Modified => data.Entity.UpdatedDate = DateTime.UtcNow,
                    _ => DateTime.UtcNow
                };
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
