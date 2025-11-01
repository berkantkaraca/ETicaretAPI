using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Application.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ETicaretAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;

        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        [HttpGet]
        public async Task Get()
        {
            //await _productWriteRepository.AddRangeAsync(new()
            //{
            //    new() { Id = Guid.NewGuid(), Name = "Product 1", Price = 100, Stock = 10 },
            //    new() { Id = Guid.NewGuid(), Name = "Product 2", Price = 200, Stock = 20 },
            //    new() { Id = Guid.NewGuid(), Name = "Product 3", Price = 300, Stock = 30 }
            //});
            //await _productWriteRepository.SaveAsync();

            //var products = _productReadRepository.GetAll();
            //return Ok(products);

            var p = await _productReadRepository.GetByIdAsync("be7cd9ef-7ba0-4193-9439-08eac134c774", false );

            p.Name = "mehmet";

            _productWriteRepository.SaveAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var product = await _productReadRepository.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);

        }
    }
}
