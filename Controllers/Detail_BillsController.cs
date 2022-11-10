using Microsoft.AspNetCore.Mvc;
using ctds_webapi.Services;
using ctds_webapi.Models;

namespace ctds_webapi.Controllers;

[ApiController]
[Route("[controller]")]

public class Detail_BillsController : ControllerBase
{
    IDetail_BillService detailBillService;

    public Detail_BillsController(IDetail_BillService service)
    {
        detailBillService = service;
    }

    [HttpGet]
    public IActionResult Get()
    {

            return Ok(detailBillService.Get());
    }

    [HttpGet("bestseller")]
    public IActionResult BestSeller()
    {
        return Ok(detailBillService.BestSellerProduct());
    }

    [HttpPost]
    public IActionResult Post([FromBody] Detail_Bill detailBill)
    {
        detailBillService.Save(detailBill);
        return Ok();
        // return CreatedAtRoute(nameof(GetPrescriptionById), new Object{Id = prescriptionDto.Id}, prescriptionDto)

    }

    [HttpPut]
    public IActionResult Put([FromRoute] Guid id, [FromBody] Detail_Bill detailBill)
    {
        detailBillService.Update(id, detailBill);
        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete([FromRoute] Guid id)
    {
        detailBillService.Delete(id);
        return Ok();
    }
}