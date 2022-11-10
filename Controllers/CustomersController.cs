using Microsoft.AspNetCore.Mvc;
using ctds_webapi.Services;
using ctds_webapi.Models;

namespace ctds_webapi.Controllers;

[ApiController]
[Route("[controller]")]

public class CustomersController : ControllerBase
{
    ICustomerService customerService;

    public CustomersController(ICustomerService service)
    {
        customerService = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(customerService.Get());
    }

    [HttpGet("consumptions")]
    public IActionResult Consumptions([FromQuery] double givenValue)
    {
        return Ok(customerService.CustomersConsumptions(givenValue));
    }

    [HttpPost]
    public IActionResult Post([FromBody] Customer customer)
    {
        customerService.Save(customer);
        return Ok();
        // return CreatedAtRoute(nameof(GetPrescriptionById), new Object{Id = prescriptionDto.Id}, prescriptionDto)

    }

    [HttpPut]
    public IActionResult Put([FromRoute] Guid id, [FromBody] Customer customer)
    {
        customerService.Update(id, customer);
        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete([FromRoute] Guid id)
    {
        customerService.Delete(id);
        return Ok();
    }
}