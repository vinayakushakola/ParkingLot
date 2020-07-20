using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using ParkingLotBusinessLayer.Interfaces;
using ParkingLotCommonLayer.ModelDB;
using ParkingLotCommonLayer.RequestModels;

namespace ParkingLot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingController : ControllerBase
    {
        private readonly IParkingBusiness _parkingBusiness;
        private static bool success = false;
        private static string message;

        public ParkingController(IParkingBusiness parkingBusiness)
        {
            _parkingBusiness = parkingBusiness;
        }

        [HttpGet]
        public IActionResult AvailableParkingSlots()
        {
            try
            {
                var data = _parkingBusiness.AvailableParkingSlots();
                if (data != null)
                {
                    success = true;
                    message = "Available Parking Slots";
                    return Ok(new { success, message, data });
                }
                else
                {
                    message = "Parking Slot is Full";
                    return Ok(new { success, message });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpGet]
        [Route("ParkedCars")]
        [Authorize]
        public IActionResult GetListOfParkedCars()
        {
            try
            {
                var user = HttpContext.User;
                if ((user.HasClaim(u => u.Type == "TokenType")) && (user.HasClaim(u => u.Type == "UserRole")))
                {
                    if ((user.Claims.FirstOrDefault(u => u.Type == "TokenType").Value == "Login") &&
                            ((user.Claims.FirstOrDefault(u => u.Type == "UserRole").Value == "Security") ||
                            (user.Claims.FirstOrDefault(u => u.Type == "UserRole").Value == "Police"))) 
                    {
                        int ID = Convert.ToInt32(user.Claims.FirstOrDefault(u => u.Type == "SecurityID").Value);
                        var data = _parkingBusiness.ListOfParkedVehicle(ID);
                        if (data != null)
                        {
                            success = true;
                            message = "Parked Cars Data Fetched Successfully";
                            return Ok(new { success, message, data });
                        }
                        else
                        {
                            message = "No cars are Parked";
                            return Ok(new { success, message });
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

        [HttpGet]
        [Route("UnParkedCars")]
        public IActionResult GetListOfUnParkedCars()
        {
            try
            {
                var user = HttpContext.User;
                if ((user.HasClaim(u => u.Type == "TokenType")) && (user.HasClaim(u => u.Type == "UserRole")))
                {
                    if ((user.Claims.FirstOrDefault(u => u.Type == "TokenType").Value == "Login") &&
                            ((user.Claims.FirstOrDefault(u => u.Type == "UserRole").Value == "Security") ||
                            (user.Claims.FirstOrDefault(u => u.Type == "UserRole").Value == "Police")))
                    {
                        int ID = Convert.ToInt32(user.Claims.FirstOrDefault(u => u.Type == "SecurityID").Value);
                        var data = _parkingBusiness.ListOfUnParkedVehicle(ID);
                        if (data != null)
                        {
                            success = true;
                            message = "UnParked Cars Data Fetched Successfully";
                            return Ok(new { success, message, data });
                        }
                        else
                        {
                            message = "No cars are UnParked";
                            return Ok(new { success, message });
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

        [HttpPost]
        [Route("Park")]
        [Authorize]
        public IActionResult ParkVehicle(ParkRequest parkDetails)
        {
            try
            {
                var user = HttpContext.User;
                if ((user.HasClaim(u => u.Type == "TokenType")) && (user.HasClaim(u => u.Type == "UserRole")))
                {
                    if ((user.Claims.FirstOrDefault(u => u.Type == "TokenType").Value == "Login") &&
                            (user.Claims.FirstOrDefault(u => u.Type == "UserRole").Value == "Security"))
                    {
                        int securityID = Convert.ToInt32(user.Claims.FirstOrDefault(u => u.Type == "SecurityID").Value);
                        var data = _parkingBusiness.ParkVehicle(securityID, parkDetails);
                        if (data != null)
                        {
                            success = true;
                            message = "Car Parked Successfully";
                            return Ok(new { success, message, data });
                        }
                        else
                        {
                            message = "Parking Slot is Full";
                            return Ok(new { success, message });
                        }
                    }
                }
                message = "Invalid Token!";
                return BadRequest(new { success, message });
            }
            catch(Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        [HttpPost]
        [Route("{receiptNo}/UnPark")]
        [Authorize]
        public IActionResult UnParkVehicle(int receiptNo)
        {
            try
            {
                var user = HttpContext.User;
                if ((user.HasClaim(u => u.Type == "TokenType")) && (user.HasClaim(u => u.Type == "UserRole")))
                {
                    if ((user.Claims.FirstOrDefault(u => u.Type == "TokenType").Value == "Login") &&
                            (user.Claims.FirstOrDefault(u => u.Type == "UserRole").Value == "Security"))
                    {
                        int securityID = Convert.ToInt32(user.Claims.FirstOrDefault(u => u.Type == "SecurityID").Value);
                        var data = _parkingBusiness.UnParkVehicle(securityID, receiptNo);
                        if (data != null)
                        {
                            success = true;
                            message = "Car UnParked Successfully";
                            return Ok(new { success, message, data });
                        }
                        else
                        {
                            message = "Car Not exists";
                            return Ok(new { success, message });
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
    }
}