using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ParkingLotCommonLayer.RequestModels
{
    public abstract class RegistrationRequest
    {
        [Required]
        [MinLength(3, ErrorMessage = "Your FirstName Length Should be more than 3")]
        public string FirstName { set; get; }

        [Required]
        [MinLength(3, ErrorMessage = "Your LastName Length Should be more than 3")]
        public string LastName { set; get; }

        [Required]
        [EmailAddress(ErrorMessage = "Please Input a Proper Email-ID")]
        public string Email { set; get; }
    }

    public class UserRegistrationRequest : RegistrationRequest
    {
        [Required]
        [MinLength(8, ErrorMessage = "Your Password Should be Minimum Length of 8")]
        public string Password { set; get; }
    }

    public class AdminRegistrationRequest : RegistrationRequest
    {
        [Required]
        [MinLength(5, ErrorMessage = "Your Password Should be Minimum Length of 5")]
        public string Password { set; get; }
    }

    public class LoginRequest
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please Input a Proper Email-ID")]
        public string Email { set; get; }

        [Required]
        public string Password { set; get; }
    }


}
