using Microsoft.AspNetCore.Mvc;
using ctds_webapi.Services;
using ctds_webapi.Models;
using AutoMapper;


namespace ctds_webapi.Controllers;

[ApiController]
[Route("[controller]")]

public class ManagersController : ControllerBase
{
    IManagerService managerService;

    IMapper mapper;

    private readonly ILogger<ManagersController> _logger;

    public ManagersController(IManagerService service, IMapper _mapper, ILogger<ManagersController> logger)
    {
        managerService = service;
        mapper = _mapper;
        _logger = logger;
    }

    [HttpGet]
    // public async Task<IActionResult> Get()
    public IActionResult Get()
    {
        try
        {
            var results = managerService.Get();
            var managers = mapper.Map<IEnumerable<ManagerDTO>>(results);
            return Ok(managers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GET attempt in {nameof(Get)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }

    [HttpGet("{id:Guid}", Name = "GetManager")]
    public IActionResult GetManager([FromRoute] Guid id)
    {
        try
        {
            var result = managerService.GetById(id);
            var manager = mapper.Map<ManagerDTO>(result);
            return Ok(manager);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GetById attempt in {nameof(GetManager)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }


    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateManagerDTO managerDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(Post)} => [Invalid ModelState: {ModelState}]");
                return BadRequest(ModelState);
            }
            Manager manager = mapper.Map<Manager>(managerDTO);
            await managerService.Save(manager);

            return CreatedAtRoute(nameof(GetManager), new { id = manager.Id }, manager);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(Post)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }

    [HttpPut("{id:Guid}")]
    public IActionResult Put([FromRoute] Guid id, [FromBody] UpdateManagerDTO managerDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(Put)} => [Invalid ModelState: {ModelState}]");
                return BadRequest(ModelState);
            }

            Manager manager = mapper.Map<Manager>(managerDTO);
            managerService.Update(id, manager);
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
            managerService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid DELETE attempt in {nameof(Delete)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }
}