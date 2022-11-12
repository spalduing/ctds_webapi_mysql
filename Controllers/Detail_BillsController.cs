using Microsoft.AspNetCore.Mvc;
using ctds_webapi.Services;
using ctds_webapi.Models;
using AutoMapper;


namespace ctds_webapi.Controllers;

[ApiController]
[Route("[controller]")]

public class Detail_BillsController : ControllerBase
{
    IDetail_BillService detailBillService;

    IMapper mapper;

    private readonly ILogger<Detail_BillsController> _logger;

    public Detail_BillsController(IDetail_BillService service, IMapper _mapper, ILogger<Detail_BillsController> logger)
    {
        detailBillService = service;
        mapper = _mapper;
        _logger = logger;
    }

    [HttpGet]
    // public async Task<IActionResult> Get()
    public IActionResult Get()
    {
        try
        {
            var results = detailBillService.Get();
            var detailBills = mapper.Map<IEnumerable<Detail_BillDTO>>(results);
            return Ok(detailBills);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GET attempt in {nameof(Get)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }

    [HttpGet("{id:Guid}", Name = "GetDetail_Bill")]
    public IActionResult GetDetail_Bill([FromRoute] Guid id)
    {
        try
        {
            var result = detailBillService.GetById(id);
            var detailBill = mapper.Map<Detail_BillDTO>(result);
            return Ok(detailBill);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GetById attempt in {nameof(GetDetail_Bill)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }
    }

    [HttpGet("bestseller")]
    public IActionResult BestSellerProduct()
    {
        try
        {
            return Ok(detailBillService.BestSellerProduct());
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GET attempt in {nameof(BestSellerProduct)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateDetail_BillDTO detailBillDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(Post)} => [Invalid ModelState: {ModelState}]");
                return BadRequest(ModelState);
            }
            Detail_Bill detailBill = mapper.Map<Detail_Bill>(detailBillDTO);
            await detailBillService.Save(detailBill);

            return CreatedAtRoute(nameof(GetDetail_Bill), new { id = detailBill.DetailBilId }, detailBill);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(Post)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }
    }

    [HttpPut("{id:Guid}")]
    public IActionResult Put([FromRoute] Guid id, [FromBody] UpdateDetail_BillDTO detailBillDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(Put)} => [Invalid ModelState: {ModelState}]");
                return BadRequest(ModelState);
            }

            Detail_Bill detailBill = mapper.Map<Detail_Bill>(detailBillDTO);
            detailBillService.Update(id, detailBill);
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
            detailBillService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid DELETE attempt in {nameof(Delete)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }
    }
}

