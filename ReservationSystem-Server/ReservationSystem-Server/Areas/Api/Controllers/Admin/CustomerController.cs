using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationSystem_Server.Data;
using ReservationSystem_Server.Services;

namespace ReservationSystem_Server.Areas.Api.Controllers.Admin;

[ApiController]
[Route($"{ApiInfo.Base}admin/[controller]")]
[Authorize(Roles = "Employee")]
public class CustomerController : ControllerBase
{
    private readonly CustomerManager _customerManager;
    
    public CustomerController(CustomerManager customerManager)
    {
        _customerManager = customerManager;
    }

    /// <summary>
    /// Gets a customer by id
    /// </summary>
    /// <param name="id">The id of the customer to fetch</param>
    /// <response code="200">Returns the customer</response>
    /// <response code="404">If the customer is not found</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Customer), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        Customer? customer = await _customerManager.GetByIdAsync(id);
        
        if (customer == null)
        {
            return NotFound();
        }

        return Ok(customer);
    }
}