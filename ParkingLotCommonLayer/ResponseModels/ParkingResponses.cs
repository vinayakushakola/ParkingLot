using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ParkingLotCommonLayer.ResponseModels
{
    public class ParkResponse
    {
        public int ReceiptNumber { get; set; }

        public string OwnerName { get; set; }

        public string VehicleNumber { get; set; }

        public string VehicleBrand { get; set; }

        public string Color { get; set; }

        public string AttendantName { get; set; }

        public bool Disability { get; set; }

        public DateTime ParkingDate { get; set; }

        public string ParkingSlot { get; set; }
    }

    public class UnParkResponse : ParkResponse
    {
        public bool IsParked { get; set; }
        public DateTime UnParkedDate { get; set; }

        public double TotalTime { get; set; }

        public double TotalAmt { get; set; }
    }

    public class ParkingSlotResponse
    {
        public ParkingSlots ParkingSlots { get; set; }
    }

    public class ParkingSlots
    {
        public int Parking_Slot_A { get; set; }
        public int Parking_Slot_B { get; set; }
        public int Parking_Slot_C { get; set; }
    }
}
