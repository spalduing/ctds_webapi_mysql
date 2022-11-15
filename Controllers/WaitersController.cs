using Microsoft.AspNetCore.Mvc;
using ctds_webapi.Services;
using ctds_webapi.Models;
using AutoMapper;


namespace ctds_webapi.Controllers;

[ApiController]
[Route("[controller]")]

public class WaitersController : ControllerBase
{
    IWaiterService waiterService;

    IMapper mapper;

    private readonly ILogger<WaitersController> _logger;

    public WaitersController(IWaiterService service, IMapper _mapper, ILogger<WaitersController> logger)
    {
        waiterService = service;
        mapper = _mapper;
        _logger = logger;
    }

    [HttpGet]
    // public async Task<IActionResult> Get()
    public IActionResult Get()
    {
        try
        {
            var results = waiterService.Get();
            var waiters = mapper.Map<IEnumerable<WaiterDTO>>(results);
            return Ok(waiters);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GET attempt in {nameof(Get)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }
    [HttpGet("{id:Guid}", Name = "GetWaiter")]
    public IActionResult GetWaiter([FromRoute] Guid id)
    {
        try
        {
            var result = waiterService.GetById(id);
            var waiter = mapper.Map<WaiterDTO>(result);
            return Ok(waiter);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GetById attempt in {nameof(GetWaiter)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }

    [HttpGet("totalSells")]
    public IActionResult TotalSells([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
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
            return Ok(waiterService.TotalSells(startDate, endDate));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GET attempt in {nameof(TotalSells)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateWaiterDTO waiterDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(Post)} => [Invalid ModelState: {ModelState}]");
                return BadRequest(ModelState);
            }
            Waiter waiter = mapper.Map<Waiter>(waiterDTO);
            await waiterService.Save(waiter);

            return CreatedAtRoute(nameof(GetWaiter), new { id = waiter.Id }, waiter);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(Post)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }

    [HttpPut("{id:Guid}")]
    public IActionResult Put([FromRoute] Guid id, [FromBody] UpdateWaiterDTO waiterDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(Put)} => [Invalid ModelState: {ModelState}]");
                return BadRequest(ModelState);
            }

            Waiter waiter = mapper.Map<Waiter>(waiterDTO);
            waiterService.Update(id, waiter);
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
            waiterService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid DELETE attempt in {nameof(Delete)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }
}