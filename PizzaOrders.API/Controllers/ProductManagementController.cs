using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain;

namespace PizzaOrders.API.Controllers
{
    [ApiController]
    [Route("api/management/products")]
    [Authorize(Roles = UserRolesConstants.AdminRole)]
    public class ProductManagementController : ControllerBase
    {
        private readonly IProductManagementService _productManagementService;

        public ProductManagementController(IProductManagementService productManagementService)
        {
            _productManagementService = productManagementService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestDto request)
        {
            var result = await _productManagementService.CreateProductAsync(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequestDto request)
        {
            try
            {
                var result = await _productManagementService.UpdateProductAsync(request);
                return Ok(result);
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await _productManagementService.DeleteProductAsync(id);
                return NoContent();
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
