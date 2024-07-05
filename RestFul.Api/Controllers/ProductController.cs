using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Restful.API.Models;
using Restful.API.Services;
using Restful.API.Validator;
using System.Net;

namespace Restful.API.Controllers
{
    public enum SortOption
    {
        None,
        Name,
        Price
    }

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var products = _productService.GetAll();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var product = _productService.GetById(id);
                if (product == null) return NotFound();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest(new { status = 400, message = "Geçersiz ürün." });
                }
                ProductValidator validator = new ProductValidator();

                var validationResult = validator.Validate(product);

                if (!validationResult.IsValid)
                {
                    return BadRequest(new { status = 400, message = validationResult.Errors });
                }

                _productService.Add(product);

                return Ok(new { status = 200, message = "İşlem başarı ile gerçekleşti" });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Product product)
        {
            try
            {
                if (product == null || product.Id == 0) return BadRequest();
                var existingProduct = _productService.GetById(product.Id);
                if (existingProduct == null) return NotFound();
                _productService.Update(product);
                return Ok(new { message = "Product updated successfully", product });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpPatch]
        public IActionResult Patch(int id, [FromBody] Product product)
        {
            try
            {
                if (product == null || product.Id == 0) return BadRequest();
                var existingProduct = _productService.GetById(product.Id);
                if (existingProduct == null) return NotFound();
                if (!string.IsNullOrEmpty(product.Name)) existingProduct.Name = product.Name;
                if (!string.IsNullOrEmpty(product.Description)) existingProduct.Description = product.Description;
                if (product.Price != 0) existingProduct.Price = product.Price;
                _productService.Update(existingProduct);
                return Ok(new { message = "Product updated successfully", product });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id == 0) return BadRequest();
                var product = _productService.GetById(id);
                if (product == null) return NotFound();
                _productService.Delete(id);
                return Ok(new { message = "Product deleted successfully", product });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("getSortedProducts")]
        public IActionResult GetSortedProducts([FromQuery] string name, [FromQuery] SortOption sort = SortOption.None)
        {
            try
            {
                var productList = _productService.GetAll();
                if (!string.IsNullOrEmpty(name))
                {
                    productList = productList.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
                }

                switch (sort)
                {
                    case SortOption.Name:
                        productList = productList.OrderBy(p => p.Name);
                        break;
                    case SortOption.Price:
                        productList = productList.OrderBy(p => p.Price);
                        break;
                    default:
                        break;
                }

                return Ok(productList);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("priceRange")]
        public IActionResult GetByPriceRange(decimal minPrice, decimal maxPrice)
        {
            try
            {
                var products = _productService.GetByCondition(p => p.Price >= minPrice && p.Price <= maxPrice);
                return Ok(new { message = "Product list realized", products });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
    }
}
