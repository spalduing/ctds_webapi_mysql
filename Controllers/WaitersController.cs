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

    public WaitersController(IWaiterService service, IMapper _mapper)
    {
        waiterService = service;
        mapper = _mapper;
    }

    [HttpGet]
    // public async Task<IActionResult> Get()
    public IActionResult Get()
    {
        var results = waiterService.Get();
        var waiters = mapper.Map<IEnumerable<WaiterDTO>>(results);
        return Ok(waiters);
        // return Ok(waiterService.Get());
    }

    [HttpGet("totalSells")]
    public IActionResult TotalSells()
    {
        return Ok(waiterService.TotalSells());
    }
    // [HttpGet("{id}",Name="GetPrescriptionById")]
    // public IActionResult GetPrescriptionById()
    // {
    //     return Ok(waiterService.GetPrescriptionById());
    // }

    [HttpPost]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> Post([FromBody] CreateWaiterDTO waiterDTO)
    {
        Waiter waiter = mapper.Map<Waiter>(waiterDTO);
        await waiterService.Save(waiter);
        return Ok();

        // return CreatedAtRoute(nameof(GetPrescriptionById), new Object{Id = prescriptionDto.Id}, prescriptionDto)
    }

    [HttpPut]
    public IActionResult Put([FromRoute] Guid id, [FromBody] Waiter waiter)
    {
        waiterService.Update(id, waiter);
        return Ok();
    }

    [HttpDelete]
    public IActionResult Delete([FromRoute] Guid id)
    {
        waiterService.Delete(id);
        return Ok();
    }
}