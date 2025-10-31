using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;
using ETicaretAPI.Persistence.Repositories;

namespace ETicaretAPI.Application.Repositories
{
    public class ProductReadRepository : ReadRepository<Product>, IProductReadRepository
    {
        public ProductReadRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
