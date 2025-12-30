using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Application.Interfaces;
using PizzaOrders.Domain;

namespace PizzaOrders.API.Controllers
{
    [ApiController]
    [Route("api/management/toppings")]
    [Authorize(Roles = UserRolesConstants.AdminRole)]
    public class ToppingManagementController : ControllerBase
    {
        private readonly IToppingManagementService _toppingManagementService;

        public ToppingManagementController(IToppingManagementService toppingManagementService)
        {
            _toppingManagementService = toppingManagementService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllToppings()
        {
            var result = await _toppingManagementService.GetAllToppingsAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopping([FromBody] CreateToppingRequestDto request)
        {
            var result = await _toppingManagementService.CreateToppingAsync(request);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTopping([FromBody] UpdateToppingRequestDto request)
        {
            try
            {
                var result = await _toppingManagementService.UpdateToppingAsync(request);
                return Ok(result);
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTopping(int id)
        {
            try
            {
                await _toppingManagementService.DeleteToppingAsync(id);
                return NoContent();
            }
            catch (System.InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
