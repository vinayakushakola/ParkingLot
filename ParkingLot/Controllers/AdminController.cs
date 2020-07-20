using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ParkingLotBusinessLayer.Interfaces;
using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;

namespace ParkingLot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminBusiness _adminBusiness;
        private readonly IConfiguration _configuration;

        private static readonly string _login = "Login";
        private static bool success = false;
        private static string message;
        private static string token;

        public AdminController(IAdminBusiness adminBusiness, IConfiguration configuration)
        {
            _adminBusiness = adminBusiness;
            _configuration = configuration;
        }

        /// <summary>
        /// Shows List of Securities Details
        /// </summary>
        /// <returns>If Data Found return Ok else Not Found or Bad Request</returns>
        [HttpGet]
        [Route("Security")]
        [Authorize]
        public IActionResult ListOfSecurity()
        {
            try
            {
                var user = HttpContext.User;
                if ((user.HasClaim(u => u.Type == "TokenType")) && (user.HasClaim(u => u.Type == "UserRole")))
                {
                    if ((user.Claims.FirstOrDefault(u => u.Type == "TokenType").Value == "Login") &&
                            (user.Claims.FirstOrDefault(u => u.Type == "UserRole").Value == "Admin"))
                    {
                        int ID = Convert.ToInt32(user.Claims.FirstOrDefault(u => u.Type == "AdminID").Value);
                        var data = _adminBusiness.ListOfSecurity(ID);
                        if (data != null)
                        {
                            success = true;
                            message = "List of Securities Data Fetched Successfully";
                            return Ok(new { success, message, data });
                        }
                        else
                        {
                            message = "No Security Data Found";
                            return NotFound(new { success, message });
                        }
                    }
                }
                message = "Invalid Token!";
                return BadRequest(new { success, message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
        /// <summary>
        /// Registration
        /// </summary>
        /// <param name="adminDetails">Admin Detials</param>
        /// <returns>If registration is Successfull return Ok 200</returns>
        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(AdminRegistrationRequest adminDetails)
        {
            try
            {
                var data = _adminBusiness.Registration(adminDetails);

                if (data == null)
                {
                    message = "No Data Provided";
                    return Ok(new { success, message });
                }
                else
                {
                    success = true;
                    message = "Admin Account Created Successfully";
                    return Ok(new { success, message, data });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="login">Login Details</param>
        /// <returns>If Login is Successfull or else 404</returns>
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginRequest login)
        {
            try
            {
                var data = _adminBusiness.Login(login);
                if (data == null)
                {
                    message = "No Admin Present with this Email-ID and Password";
                    return Ok(new { success, message });
                }
                else
                {
                    success = true;
                    message = "User Successfully Logged In";
                    token = GenerateToken(data, _login);
                    return Ok(new { success, message, data, token });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { e.Message });
            }
        }

        /// <summary>
        /// It Generate the token.
        /// </summary>
        /// <param name="adminDetails">Response Model</param>
        /// <param name="tokenType">Token Type</param>
        /// <returns>It return Token</returns>
        private string GenerateToken(RegistrationResponse adminDetails, string tokenType)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("AdminID", adminDetails.ID.ToString()),
                    new Claim("Email", adminDetails.Email.ToString()),
                    new Claim("TokenType", tokenType),
                    new Claim("UserRole", adminDetails.UserRole.ToString())
                };

                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"],
                    claims, expires: DateTime.Now.AddDays(1), signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}