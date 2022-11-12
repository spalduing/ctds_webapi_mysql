using Microsoft.AspNetCore.Mvc;
using ctds_webapi.Services;
using ctds_webapi.Models;
using AutoMapper;


namespace ctds_webapi.Controllers;

[ApiController]
[Route("[controller]")]

public class TablesController : ControllerBase
{
    ITableService tableService;

    IMapper mapper;

    private readonly ILogger<TablesController> _logger;

    public TablesController(ITableService service, IMapper _mapper, ILogger<TablesController> logger)
    {
        tableService = service;
        mapper = _mapper;
        _logger = logger;
    }

    [HttpGet]
    // public async Task<IActionResult> Get()
    public IActionResult Get()
    {
        try
        {
            var results = tableService.Get();
            var tables = mapper.Map<IEnumerable<TableDTO>>(results);
            return Ok(tables);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GET attempt in {nameof(Get)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }

    [HttpGet("{id:Guid}", Name = "GetTable")]
    public IActionResult GetTable([FromRoute] Guid id)
    {
        try
        {
            var result = tableService.GetById(id);
            var table = mapper.Map<TableDTO>(result);
            return Ok(table);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid GetById attempt in {nameof(GetTable)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }


    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateTableDTO tableDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(Post)} => [Invalid ModelState: {ModelState}]");
                return BadRequest(ModelState);
            }
            Table table = mapper.Map<Table>(tableDTO);
            await tableService.Save(table);

            return CreatedAtRoute(nameof(GetTable), new { id = table.TableId }, table);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid POST attempt in {nameof(Post)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }

    [HttpPut("{id:Guid}")]
    public IActionResult Put([FromRoute] Guid id, [FromBody] UpdateTableDTO tableDTO)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(Put)} => [Invalid ModelState: {ModelState}]");
                return BadRequest(ModelState);
            }

            Table table = mapper.Map<Table>(tableDTO);
            tableService.Update(id, table);
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
            tableService.Delete(id);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Invalid DELETE attempt in {nameof(Delete)} => [{ex}]");
            return StatusCode(500, "Internal Server Error. Please Try Again Later");
        }

    }
}