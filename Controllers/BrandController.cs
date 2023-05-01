using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingApp.Interfaces;
using ShoppingApp.Models;
using ShoppingApp.Models.Dto;

namespace ShoppingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;

        public BrandController(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBrands()
        {
            var brands = _mapper.Map<List<BrandDto>>(_brandRepository.GetBrands());

            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            return Ok(brands);
        }

        [HttpGet("{id}")]
        public IActionResult GetBrand(int id)
        {
            if (!_brandRepository.BrandExists(id))
            {
                return NotFound();
            }

            var brand = _mapper.Map<BrandDto>(_brandRepository.GetBrand(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(brand);
        }

        [HttpGet("product/{brandId}")]
        public IActionResult GetProductsByBrandId(int brandId)
        {
            var products = _mapper.Map<List<ProductDto>>(_brandRepository.GetProductsByBrandId(brandId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(products);
        }

        [HttpPost]
        public IActionResult CreateBrand([FromBody] BrandDto brandCreate)
        {
            if (brandCreate == null)
            {
                return BadRequest(ModelState);
            }

            var brand = _brandRepository.GetBrands().FirstOrDefault(b => b.Name.Trim().ToUpper() == brandCreate.Name.Trim().ToUpper());

            if (brand != null)
            {
                ModelState.AddModelError("", "Brand already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brandMap = _mapper.Map<Brand>(brandCreate);

            if (!_brandRepository.CreateBrand(brandMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{brandId}")]
        public IActionResult UpdateBrand(int brandId, [FromBody] BrandDto updatedBrand)
        {
            if (updatedBrand == null)
            {
                return BadRequest(ModelState);
            }

            if (brandId != updatedBrand.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_brandRepository.BrandExists(brandId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brandMap = _mapper.Map<Brand>(updatedBrand);

            if (!_brandRepository.UpdateBrand(brandMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{brandId}")]
        public IActionResult DeleteBrand(int brandId)
        {
            if (!_brandRepository.BrandExists(brandId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var brandToDelete = _brandRepository.GetBrand(brandId);

            if (!_brandRepository.DeleteBrand(brandToDelete))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
