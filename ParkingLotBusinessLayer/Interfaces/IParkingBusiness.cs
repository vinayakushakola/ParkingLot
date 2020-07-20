using ParkingLotCommonLayer.ModelDB;
using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotBusinessLayer.Interfaces
{
    public interface IParkingBusiness
    {
        ParkingSlotResponse AvailableParkingSlots();

        ParkResponse GetParkedCarDetailsbyReceiptNo(int receiptNo);

        ParkResponse GetParkedCarDetailsbyVehicleNo(string vehicleNo);

        List<ParkResponse> ListOfParkedVehicle(int id);

        List<UnParkResponse> ListOfUnParkedVehicle(int id);

        ParkResponse ParkVehicle(int securityID, ParkRequest parkDetails);

        UnParkResponse UnParkVehicle(int securityID, int receiptNo);
    }
}
