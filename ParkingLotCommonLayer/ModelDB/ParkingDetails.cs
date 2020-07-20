using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ParkingLotCommonLayer.ModelDB
{
    public class ParkingDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReceiptNumber { get; set; }

        [RegularExpression(@"^[A-Z][a-zA-Z]*$")]
        [Required]
        public string OwnerName { get; set; }

        [Required]
        public string VehicleNumber { get; set; }
        
        [Required]
        public string VehicleBrand { get; set; }
        
        [Required]
        public string Color { get; set; }

        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$")]
        public string AttendantName { get; set; }

        [DefaultValue("false")]
        public bool IsParked { get; set; }

        [DefaultValue("false")]
        public bool Disability { get; set; }

        [Required]
        public DateTime ParkingDate { get; set; }

        [RegularExpression(@"^([A-Za-z]){1,2}$")]
        public string ParkingSlot { get; set; }
    }
}
