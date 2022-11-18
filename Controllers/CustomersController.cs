using Microsoft.AspNetCore.Mvc;
using ctds_webapi.Services;
using ctds_webapi.Models;
using AutoMapper;


namespace ctds_webapi.Controllers;

[ApiController]
[Route("[controller]")]

public class CustomersController : ControllerBase
{
    ICustomerService customerService;

    IMapper mapper;

    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerService service, IMapper _mapper, ILogger<CustomersController> logger)
    {
        customerService = service;
        mapper = _mapper;
        _logger = logger;
    }

    [HttpGet]
    // public async Task<IActionResult> Get()
    public IActionResult Get()
    {
        try
        {
            var results = customerService.Get();
            var customers = mapper.Map<IEnumerable<CustomerDTO>>(results);
            return Ok(customers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GET attempt in {nameof(Get)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }
    [HttpGet("{id:Guid}", Name = "GetCustomer")]
    public IActionResult GetCustomer([FromRoute] Guid id)
    {
        try
        {
            var result = customerService.GetById(id);
            var customer = mapper.Map<CustomerDTO>(result);
            return Ok(customer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GetById attempt in {nameof(GetCustomer)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }

    [HttpGet("consumptions")]
    // public IActionResult Consumptions([FromQuery] double givenValue, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    public async Task<IActionResult> Consumptions([FromQuery] double givenValue, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var startQueryParam = Request.Query["startDate"];
            var endQueryParam = Request.Query["endDate"];
            // IMPORTANT_COMMENT: IF NO QUERY PARAMS FOR DATE RANGE IS PROVIDED,
            // IT RETURNS A DEFAULT RANGE FROM THE FIRST DAY OF THE CURRENT YEAR TO NOW
            if( string.IsNullOrEmpty(startQueryParam)  ||  string.IsNullOrEmpty(endQueryParam))
            {
            DateTime now = DateTime.Now;
            startDate = new DateTime(now.Year, 1, 1);
            endDate = new DateTime(now.Year, now.Month, now.Day);
            }
            var consumptions = await customerService.CustomersConsumptions(givenValue, startDate, endDate);
            return Ok(consumptions);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GET attempt in {nameof(Consumptions)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateCustomerDTO customerDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(Post)} => [Invalid ModelState: {ModelState}]");
                return BadRequest(ModelState);
            }
            Customer customer = mapper.Map<Customer>(customerDTO);
            Console.Write(customer);
            await customerService.Save(customer);

            return CreatedAtRoute(nameof(GetCustomer), new { id = customer.Id }, customer);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(Post)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }

    [HttpPut("{id:Guid}")]
    public IActionResult Put([FromRoute] Guid id, [FromBody] UpdateCustomerDTO customerDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(Put)} => [Invalid ModelState: {ModelState}]");
                return BadRequest(ModelState);
            }

            Customer customer = mapper.Map<Customer>(customerDTO);
            customerService.Update(id, customer);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid UPDATE attempt in {nameof(Put)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }

    [HttpDelete("{id:Guid}")]
    public IActionResult Delete([FromRoute] Guid id)
    {

        try
        {
            customerService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid DELETE attempt in {nameof(Delete)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }
}

