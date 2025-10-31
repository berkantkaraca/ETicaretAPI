using ETicaretAPI.Domain.Entities;
using ETicaretAPI.Persistence.Contexts;
using ETicaretAPI.Persistence.Repositories;

namespace ETicaretAPI.Application.Repositories
{
    public class ProductWriteRepository : WriteRepository<Product>, IProductWriteRepository
    {
        public ProductWriteRepository(ETicaretAPIDbContext context) : base(context)
        {
        }
    }
}
