using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using AccountingTool.Models;
using System.IdentityModel.Tokens.Jwt;
using Azure.Core;
using NuGet.Versioning;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountingTool.Areas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingToolController : ControllerBase
    {
        public readonly string _connectionString;
        //public readonly AccountingContext _accountingContext;
        public AccountingToolController(IConfiguration configuration /*AccountingContext accountingContext*/)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:Default");
            //_accountingContext = accountingContext;
        }

        //讀取token資料
        public string readTokenUserId(HttpRequest Request)
        {
            string authorization = Request.Headers["Authorization"];
            string jwtToken = authorization.Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(jwtToken);

            string UserId = jwtSecurityToken.Claims.First(claim => claim.Type == "UserId").Value;

            return UserId;
        }

        //取得類別列表
        public IEnumerable<Label> getLabelList()
        {
            string labelSqlString = "SELECT * FROM Labels";
            using (var conn = new SqlConnection(_connectionString))
            {
               return conn.Query<Label>(labelSqlString);
            }
        }

        //取得資料列表
        [HttpGet("getDataList")]
        public ActionResult Get(string category, string startDate, string endDate)
        {
            string userId = readTokenUserId(Request);

            var parameters = new DynamicParameters();
            parameters.Add("userId", userId);
            parameters.Add("category", category);
            parameters.Add("startDate", startDate.ToString());
            parameters.Add("endDate", endDate.ToString());

            //取得符合UserId、日期、類型的資料列表
            string dataListSqlString =
            "SELECT * FROM AccountingDatas " +
            "WHERE AccountingDatas.UserId = @userId " +
            "AND AccountingDatas.Time > @startDate " +
            "AND AccountingDatas.Time < @endDate ";

            if(category == "expense" || category == "income") {
                dataListSqlString = dataListSqlString + "AND AccountingDatas.Category = @category ";
            }

            dataListSqlString = dataListSqlString + "ORDER BY AccountingDatas.Time DESC";

            IEnumerable<AccountingDataGet> DataListResult = new List<AccountingDataGet>();
            using (var conn = new SqlConnection(_connectionString))
            {
                DataListResult = conn.Query<AccountingDataGet>(dataListSqlString, parameters);
            }

            //取得label資料列表
            IEnumerable<Label> LabelResult = getLabelList();

            //將符合dataList label 的label 資料寫入
            foreach (var data in DataListResult)
            {
                Label label = LabelResult.FirstOrDefault(x => x.Id == data.Label);
                data.LabelContent = label;
            }

            return Ok(DataListResult);
        }

        //依id取得特定資料
        [HttpGet("getData/{id}")]
        public ActionResult Get(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);
            string SqlString = "SELECT * FROM AccountingDatas WHERE Id = @Id";
            using (var conn = new SqlConnection(_connectionString))
            {
                var result = conn.Query<Models.AccountingData>(SqlString, parameters);
                return Ok(result);
            }
        }

        //新增資料
        [HttpPost]
        public ActionResult Post(Models.AccountingDataPost accountingData)
        {
            string userId = readTokenUserId(Request);
            var parameters = new DynamicParameters();
            parameters.Add("Id", null);
            parameters.Add("Description", accountingData.Description);
            parameters.Add("Category", accountingData.Category);
            parameters.Add("UserId", userId);
            parameters.Add("Label", accountingData.Label);
            parameters.Add("Time", accountingData.Time);
            parameters.Add("Price", accountingData.Price);

            string SqlString =
                "INSERT INTO AccountingDatas" +
                 "([Description], [Category], [UserId], [Label], [Time], [Price])" +
                "VALUES (@Description, @Category, @UserId, @Label, @Time, @Price)";

            using (var conn = new SqlConnection(_connectionString))
            {
                var result = conn.Execute(SqlString, parameters);
                return Ok(result);
            }
        }

        //編輯資料
        [HttpPut]
        public ActionResult Put(Models.AccountingDataPut accountingData)
        {
            string userId = readTokenUserId(Request);
            var parameters = new DynamicParameters();
            parameters.Add("Id", accountingData.Id);
            parameters.Add("Description", accountingData.Description);
            parameters.Add("Category", accountingData.Category);
            parameters.Add("UserId", userId);
            parameters.Add("Label", accountingData.Label);
            parameters.Add("Time", accountingData.Time);
            parameters.Add("Price", accountingData.Price);

            string SqlString =
                "UPDATE AccountingDatas " +
                 "SET "+
                 "[Description] = @Description ," +
                 "[Category] = @Category ," +
                 "[UserId] = @UserId ," +
                 "[Label] = @Label ," +
                 "[Time] = @Time ," +
                 "[Price] = @Price "+
                 "WHERE Id = @Id";

            using (var conn = new SqlConnection(_connectionString))
            {
                var result = conn.Execute(SqlString, parameters);
                return Ok(result);
            }
        }

        //刪除資料
        [HttpDelete("deleteData/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                string userId = readTokenUserId(Request);
                var parameters = new DynamicParameters();
                parameters.Add("Id", id);
                using (var conn = new SqlConnection(_connectionString))
                {
                    var result = conn.Execute("DELETE FROM AccountingDatas WHERE Id = @Id", parameters);
                    return Ok("成功刪除資料");
                }
            }
            catch
            {
                return BadRequest("無法刪除資料");
            }
        }

        [HttpGet("getLabelList")]
        public ActionResult getLabelListApi()
        {
            try
            {
                IEnumerable<Label> LabelList = getLabelList();
                return Ok(LabelList);
            }
            catch
            {
                return BadRequest("無法取得列表資料");
            }
        }
    }
}
