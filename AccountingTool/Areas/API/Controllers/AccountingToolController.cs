using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using AccountingTool.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountingTool.Areas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingToolController : ControllerBase
    {
        public readonly string _connectionString;
        public AccountingToolController(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:Default");
        }

        // GET: api/<AccountingDataController>
        [HttpGet]
        public ActionResult Get()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = conn.Query<Models.AccountingData>("SELECT * FROM AccountingDatas");
                return Ok(result);
            }
        }

        // GET api/<AccountingDataController>/5
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = conn.Query<Models.AccountingData>("SELECT * FROM AccountingDatas WHERE Id = @Id", parameters);
                return Ok(result);
            }
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
