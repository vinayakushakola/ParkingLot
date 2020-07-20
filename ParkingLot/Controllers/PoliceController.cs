using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
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
    public class PoliceController : ControllerBase
    {
        private readonly IPoliceBusiness _userBusiness;
        private readonly IConfiguration _configuration;

        private static readonly string _login = "Login";
        private static bool success = false;
        private static string message;
        private static string token;

        public PoliceController(IPoliceBusiness userBusiness, IConfiguration configuration)
        {
            _userBusiness = userBusiness;
            _configuration = configuration;
        }

        /// <summary>
        /// Registration
        /// </summary>
        /// <param name="userDetails">User Detials</param>
        /// <returns>If registration is Successfull return Ok 200</returns>
        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration(UserRegistrationRequest userDetails)
        {
            try
            {
                if (!ValidateRegistrationRequest(userDetails))
                    return BadRequest(new { Message = "Enter Proper Data" });

                var data = _userBusiness.Registration(userDetails);

                if (data == null)
                {
                    message = "No Data Provided";
                    return Ok(new { success, message });
                }
                else
                {
                    success = true;
                    message = "User Account Created Successfully";
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
                if (!ValidateLoginRequest(login))
                    return BadRequest(new { Message = "Enter Proper Input Value." });

                var data = _userBusiness.Login(login);
                if (data == null)
                {
                    message = "No User Present with this Email-Id and Password";
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
        /// <param name="userDetails">Response Model</param>
        /// <param name="tokenType">Token Type</param>
        /// <returns>It return Token</returns>
        private string GenerateToken(RegistrationResponse userDetails, string tokenType)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim("PoliceID", userDetails.ID.ToString()),
                    new Claim("Email", userDetails.Email.ToString()),
                    new Claim("TokenType", tokenType),
                    new Claim("UserRole", userDetails.UserRole.ToString())
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

        /// <summary>
        /// It Validate the RegisterRequest Model Data Before passing on to Business Layer
        /// </summary>
        /// <param name="userDetails">New User Data</param>
        /// <returns>Return true if validation successfull or else false</returns>
        private bool ValidateRegistrationRequest(UserRegistrationRequest userDetails)
        {
            if (userDetails == null || string.IsNullOrWhiteSpace(userDetails.FirstName) ||
                    string.IsNullOrWhiteSpace(userDetails.LastName) || string.IsNullOrWhiteSpace(userDetails.Email) ||
                    string.IsNullOrWhiteSpace(userDetails.Password) || userDetails.FirstName.Length < 3 ||
                    userDetails.LastName.Length < 3 || !userDetails.Email.Contains('@') ||
                    !userDetails.Email.Contains('.') || userDetails.Password.Length < 8)
                return false;
            else
                return true;
        }

        /// <summary>
        /// It Validate The LoginRequest Model Data Before Passing it to Business Layer.
        /// </summary>
        /// <param name="loginRequest">Login Data</param>
        /// <returns>Return True If validation is successfull or else false</returns>
        private bool ValidateLoginRequest(LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Email) ||
                string.IsNullOrWhiteSpace(loginRequest.Password) || !loginRequest.Email.Contains('@') ||
                !loginRequest.Email.Contains('.') || loginRequest.Password.Length < 8)
                return false;
            else
                return true;
        }
    }
}