using Microsoft.AspNetCore.Mvc;
using ctds_webapi.Services;
using ctds_webapi.Models;
using AutoMapper;


namespace ctds_webapi.Controllers;

[ApiController]
[Route("[controller]")]

public class BillsController : ControllerBase
{
    IBillService billService;

    IMapper mapper;

    private readonly ILogger<BillsController> _logger;

    public BillsController(IBillService service, IMapper _mapper, ILogger<BillsController> logger)
    {
        billService = service;
        mapper = _mapper;
        _logger = logger;
    }

    [HttpGet]
    // public async Task<IActionResult> Get()
    public IActionResult Get()
    {
        try
        {
            var results = billService.Get();
            var bills = mapper.Map<IEnumerable<BillDTO>>(results);
            return Ok(bills);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GET attempt in {nameof(Get)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }
    [HttpGet("{id:Guid}", Name = "GetBill")]
    public IActionResult GetBill([FromRoute] Guid id)
    {
        try
        {
            var result = billService.GetById(id);
            var bill = mapper.Map<BillDTO>(result);
            return Ok(bill);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GetById attempt in {nameof(GetBill)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }

    [HttpPost("registerBill")]
    public async Task<IActionResult> RegisterBill([FromBody] CreateBillDTO billDTO)
    {
        try
        {
            var bill = mapper.Map<Bill>(billDTO);
            billService.RegisterBill(bill);
            // return Ok();
            return CreatedAtRoute(nameof(GetBill), new { id = bill.BillId }, bill);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(RegisterBill)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }


    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateBillDTO billDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(Post)} => [Invalid ModelState: {ModelState}]");
                return BadRequest(ModelState);
            }
            Bill bill = mapper.Map<Bill>(billDTO);
            await billService.Save(bill);

            return CreatedAtRoute(nameof(GetBill), new { id = bill.BillId }, bill);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(Post)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }

    [HttpPut("{id:Guid}")]
    public IActionResult Put([FromRoute] Guid id, [FromBody] UpdateBillDTO billDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(Put)} => [Invalid ModelState: {ModelState}]");
                return BadRequest(ModelState);
            }

            Bill bill = mapper.Map<Bill>(billDTO);
            billService.Update(id, bill);
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
            billService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid DELETE attempt in {nameof(Delete)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }
}

