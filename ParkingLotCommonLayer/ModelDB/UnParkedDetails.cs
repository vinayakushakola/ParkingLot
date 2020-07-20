using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ParkingLotCommonLayer.ModelDB
{
    public class UnParkedDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("ParkingDetails")]
        public int ReceiptNumber { get; set; }

        public DateTime UnParkedDate { get; set; }

        public double TotalTime { get; set; }

        public double TotalAmt { get; set; }

    }
}
