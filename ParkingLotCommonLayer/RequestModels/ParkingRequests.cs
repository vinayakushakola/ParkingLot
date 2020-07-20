using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ParkingLotCommonLayer.RequestModels
{
    public class ParkRequest
    {
        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$")]
        public string OwnerName { get; set; }

        [Required]
        public string VehicleNumber { get; set; }

        [Required]
        public string VehicleBrand { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        [DefaultValue("false")]
        public bool Disability { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$")]
        public string AttendantName { get; set; }
    }
}
