using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.Data.SqlClient;
using AccountingTool.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountingTool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingDataController : ControllerBase
    {
        public readonly string _connectionString;
        public  AccountingDataController(IConfiguration configuration)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:Default");
        }

        // GET: api/<AccountingDataController>
        [HttpGet]
        public ActionResult Get()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = conn.Query<AccountingData>("SELECT * FROM AccountingDatas");
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
                var result = conn.Query<AccountingData>("SELECT * FROM AccountingDatas WHERE Id = @Id", parameters);
                return Ok(result);
            }
        }

        // POST api/<AccountingDataController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AccountingDataController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AccountingDataController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
