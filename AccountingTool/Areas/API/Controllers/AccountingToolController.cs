using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using AccountingTool.Models;
using System.IdentityModel.Tokens.Jwt;
using Azure.Core;
using NuGet.Versioning;
using System.Collections.Generic;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountingTool.Areas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingToolController : ControllerBase
    {
        public readonly string _connectionString;
        private readonly AccountingContext _context;
        public AccountingToolController(IConfiguration configuration, AccountingContext context)
        {
            _connectionString = configuration.GetValue<string>("ConnectionStrings:Default");
            _context = context;
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

        //取得資料列表
        [HttpGet("getDataList")]
        public ActionResult Get(string category, string startDate, string endDate)
        {
            string userId = readTokenUserId(Request);

            var DataListResult = _context.AccountingDatas
                .Where(a => a.UserId == Int32.Parse(userId))
                .Where(a => a.Time >= DateTime.Parse(startDate))
                .Where(a => a.Time <= DateTime.Parse(endDate))
                .Select(a => new AccountingDataGet
                 {
                     Id = a.Id,
                     UserId = a.UserId,
                     Description = a.Description,
                     Category = a.Category,
                     Label = a.Label,
                     LabelContent = _context.Labels.Where(b => b.Id == a.Label).FirstOrDefault(),
                     Time = a.Time,
                     Price = a.Price
                 });

            if (category == "expense" || category == "income")
            {
                DataListResult = DataListResult.Where(a => a.Category == category);
            }
            DataListResult = DataListResult.OrderByDescending(a => a.Time);

            return Ok(DataListResult);
        }

        //依id取得特定資料
        [HttpGet("getData/{id}")]
        public ActionResult Get(int id)
        {
            var data = _context.AccountingDatas.Where(a => a.Id == id);
            return Ok(data);
        }

        //新增資料
        [HttpPost]
        public ActionResult Post(Models.AccountingDataPost accountingData)
        {
            try
            {
                Models.AccountingData newData = new Models.AccountingData()
                {
                    Category = accountingData.Category,
                    UserId = Int32.Parse(readTokenUserId(Request)),
                    Label = accountingData.Label,
                    Time = accountingData.Time,
                    Price = accountingData.Price
                };
                if (!accountingData.Description.IsNullOrEmpty())
                {
                    newData.Description = accountingData.Description;
                }

                _context.Add(newData);
                _context.SaveChanges();
                return Ok("成功建立資料");
            }
            catch
            {
                return BadRequest("建立資料失敗");
            }
        }

        //編輯資料
        [HttpPut]
        public ActionResult Put(Models.AccountingDataPut accountingData)
        {
            try
            {
                Models.AccountingData updateData = _context.AccountingDatas.Where(a => a.Id == accountingData.Id).FirstOrDefault();

                if (!accountingData.Description.IsNullOrEmpty())
                {
                    updateData.Description = accountingData.Description;
                }
                else
                {
                    updateData.Description = "";
                }
                
                updateData.Category = accountingData.Category;
                updateData.UserId = Int32.Parse(readTokenUserId(Request));
                updateData.Label = accountingData.Label;
                updateData.Time = accountingData.Time;
                updateData.Price = accountingData.Price;

                _context.SaveChanges();
                return Ok("成功修改資料");
            }
            catch
            {
                return BadRequest("修改資料失敗");
            }
        }

        //刪除資料
        [HttpDelete("deleteData/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                Models.AccountingData deleteData = _context.AccountingDatas.Where(a => a.Id == id).FirstOrDefault();
                _context.AccountingDatas.Remove(deleteData);
                _context.SaveChanges();

                return Ok("成功刪除資料");
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
                IEnumerable<Label> LabelList = _context.Labels;
                return Ok(LabelList);
            }
            catch
            {
                return BadRequest("無法取得列表資料");
            }
        }
    }
}
