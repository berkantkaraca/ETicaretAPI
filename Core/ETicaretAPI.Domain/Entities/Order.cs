using ETicaretAPI.Domain.Entities.Common;

namespace ETicaretAPI.Domain.Entities
{
    public class  Order : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string OrderCode { get; set; }

        public Basket Basket { get; set; }
        public ICollection<Product> Products { get; set; } // 1 order'ın birden fazla ürünü olabilir
        public Customer Customer { get; set; } // 1 order bir müşteriye aittir
    }
}
