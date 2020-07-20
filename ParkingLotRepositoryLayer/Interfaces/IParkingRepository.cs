using ParkingLotCommonLayer.ModelDB;
using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotRepositoryLayer.Interfaces
{
    public interface IParkingRepository
    {
        ParkingSlotResponse AvailableParkingSlots();

        List<ParkResponse> ListOfParkedVehicle(int id);

        List<UnParkResponse> ListOfUnParkedVehicle(int id);

        ParkResponse ParkVehicle(int securityID, ParkRequest parkDetails);
        
        UnParkResponse UnParkVehicle(int securityID, int receiptNo);
    }
}
