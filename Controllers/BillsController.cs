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

    public BillsController(IBillService service, IMapper _mapper)
    {
        billService = service;
        mapper = _mapper;
        // return CreatedAtRoute(nameof(GetPrescriptionById), new Object{Id = prescriptionDto.Id}, prescriptionDto)

    }

    [HttpGet]
    public IActionResult Get()
    {

        return Ok(billService.Get());
    }

    [HttpPost]
    public IActionResult Post([FromBody] Bill bill)
    {
        billService.Save(bill);
        return Ok();
    }
    [HttpPost("registerBill")]
    public async Task<IActionResult> RegisterBill([FromBody] CreateBillDTO billDTO)
    {
        try
        {
            var bill = mapper.Map<Bill>(billDTO);
            billService.RegisterBill(bill);
            return Ok();
        }
        catch (Exception ex)
        {
            return Ok(ex);
        }

    }

    [HttpPut]
    public IActionResult Put([FromRoute] Guid id, [FromBody] Bill bill)
    {
        billService.Update(id, bill);
        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete([FromRoute] Guid id)
    {
        billService.Delete(id);
        return Ok();
    }
}