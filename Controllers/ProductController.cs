using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Interfaces;
using ShoppingApp.Models;
using ShoppingApp.Models.Dto;

namespace ShoppingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IBrandRepository brandRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _brandRepository = brandRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _mapper.Map<List<ProductDto>>(_productRepository.GetProducts());

            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            if (!_productRepository.ProductExists(id))
            {
                return NotFound();
            }

            var product = _mapper.Map<ProductDto>(_productRepository.GetProduct(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(product);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromQuery] int brandId, [FromQuery] int categoryId, [FromBody] ProductDto productCreate)
        {
            if (productCreate == null)
            {
                return BadRequest(ModelState);
            }

            var product = _productRepository.GetProducts().FirstOrDefault(p => p.Name.Trim().ToUpper() == productCreate.Name.Trim().ToUpper());

            if (product != null)
            {
                ModelState.AddModelError("", "Product already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productMap = _mapper.Map<Product>(productCreate);

            productMap.Brand = _brandRepository.GetBrand(brandId);
            productMap.Category = _categoryRepository.GetCategory(categoryId);

            if (!_productRepository.CreateProduct(productMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{productId}")]
        public IActionResult UpdateProduct(int productId, [FromQuery] int brandId, [FromQuery] int categoryId, [FromBody] ProductDto updatedProduct)
        {
            if (updatedProduct == null)
            {
                return BadRequest(ModelState);
            }

            if (productId != updatedProduct.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_productRepository.ProductExists(productId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productMap = _mapper.Map<Product>(updatedProduct);

            productMap.Brand = _brandRepository.GetBrand(brandId);
            productMap.Category = _categoryRepository.GetCategory(categoryId);

            if (!_productRepository.UpdateProduct(productMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{productId}")]
        public IActionResult DeleteProduct(int productId)
        {
            if (!_productRepository.ProductExists(productId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productToDelete = _productRepository.GetProduct(productId);

            if (!_productRepository.DeleteProduct(productToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
