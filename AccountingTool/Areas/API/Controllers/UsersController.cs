using Microsoft.AspNetCore.Mvc;
using AccountingTool.Models;
using System.Configuration;
using Microsoft.Data.SqlClient;
using System.Data.SqlTypes;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.IO;
using Newtonsoft.Json;
using NuGet.Common;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccountingTool.Areas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;
        public readonly string _saltString;
        private readonly AccountingContext _context;

        public UsersController (IConfiguration configuration, AccountingContext context)
        {
            _configuration = configuration;
            _connectionString = configuration.GetValue<string>("ConnectionStrings:Default");
            _saltString = configuration.GetValue<string>("SaltString");
            _context = context;
        }

        //密碼雜湊、加鹽
        public string getHashedPassword(User userData)
        {
            byte[] salt = Encoding.UTF8.GetBytes(_saltString); // divide by 8 to convert bits to bytes
            string HashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                  password: userData.Password,
                  salt: salt,
                  prf: KeyDerivationPrf.HMACSHA256,
                  iterationCount: 100000,
                  numBytesRequested: 256 / 8));

            return HashedPassword;
        }

        //確認是否為新的信箱
        public bool checkIfIsNewEmail(string Email)
        {
            var matchUser = _context.Users
                .Where(a => a.Email == Email);
            return matchUser.Count() == 0;
        }

        //建立帳號
        [HttpPost("CreateAccount")]
        public ActionResult CreateAccount(User userData)
        {
            if (checkIfIsNewEmail(userData.Email))
            {
                
                try
                {
                    string hashedPassword = getHashedPassword(userData);

                    User newUser = new User()
                    {
                        Email = userData.Email,
                        Password = hashedPassword,
                    };

                    _context.Add(newUser);
                    _context.SaveChanges();
                    return Ok("帳號建立成功");
                    //string hashedPassword = getHashedPassword(userData);

                    //string SqlString = "INSERT INTO Users([Email], [Password]) VALUES (@Email, @Password)";
                    //var parameters = new DynamicParameters();
                    //parameters.Add("Email", userData.Email);
                    //parameters.Add("Password", hashedPassword);

                    //using (var conn = new SqlConnection(_connectionString))
                    //{
                    //    var result = conn.Execute(SqlString, parameters);
                    //}

                    //return Ok("帳號建立成功");
                }
                catch
                {
                    return BadRequest("帳號建立失敗");
                }
            }
            else
            {
                return BadRequest("此Email已被註冊");
            }

        }

        // 登入
        [HttpPost("LogIn")]
        public ActionResult Get(User userData)
        {
            //查詢符合的帳號密碼
            string hashedPassword = getHashedPassword(userData);

            var matchUser = _context.Users
                .Where(a => a.Email == userData.Email)
                .Where(a => a.Password == hashedPassword);

            if(matchUser == null)
            {
                return NotFound("帳號或密碼錯誤");
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Email, matchUser.First().Email),
                    new Claim("UserId", matchUser.First().Id.ToString()),
                };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:KEY"]));

                var jwt = new JwtSecurityToken
                (
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwt);
                return Ok(jwtToken);
            }
        }

        //確認token是否有效
        [Authorize]
        [HttpPost("CheckTokenValid")]
        public void CheckTokenValid()
        {
        }
    }
}
