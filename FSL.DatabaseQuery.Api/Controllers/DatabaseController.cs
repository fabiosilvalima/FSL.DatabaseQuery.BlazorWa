using FSL.DatabaseQuery.Core.Repository;
using FSL.Framework.Core.Extensions;
using FSL.Framework.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FSL.DatabaseQuery.Api.Controllers
{
    [Route("api/database")]
    [ApiController]
    public sealed class DatabaseController : ControllerBase
    {
        private readonly IDatabaseQueryRepository _databaseQueryRepository;

        public DatabaseController(
            IDatabaseQueryRepository databaseQueryRepository)
        {
            _databaseQueryRepository = databaseQueryRepository;
        }

        [HttpPost("tables")]
        public async Task<IActionResult> PostTablesAsync(
            [FromBody] Core.Models.DatabaseRequest request)
        {
            var data = await _databaseQueryRepository.GetAllTables(request);

            return Ok(data.ToResult());
        }

        [HttpPost("columns")]
        public async Task<IActionResult> PostColumnsAsync(
            [FromBody] Core.Models.DatabaseRequest request)
        {
            var data = await _databaseQueryRepository.GetAllColumnsFromTable(
                request, 
                request.TableName);

            return Ok(data.ToResult());
        }

        [HttpPost("data")]
        public async Task<IActionResult> PostDataAsync(
            [FromBody] Core.Models.DatabaseRequest request)
        {
            var data = await _databaseQueryRepository.GetDataAsync(request);
            
            return Ok(data.ToResult());
        }
    }
}
