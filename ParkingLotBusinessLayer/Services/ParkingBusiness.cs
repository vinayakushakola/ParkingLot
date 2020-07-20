using ParkingLotBusinessLayer.Interfaces;
using ParkingLotCommonLayer.ModelDB;
using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;
using ParkingLotRepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotBusinessLayer.Services
{
    public class ParkingBusiness : IParkingBusiness
    {
        private readonly IParkingRepository _parkingRepository;

        public ParkingBusiness(IParkingRepository parkingRepository)
        {
            _parkingRepository = parkingRepository;
        }

        public ParkingSlotResponse AvailableParkingSlots()
        {
            var responseData = _parkingRepository.AvailableParkingSlots();
            return responseData;
        }

        public ParkResponse GetParkedCarDetailsbyReceiptNo(int receiptNo)
        {
            if (receiptNo <= 0)
            {
                return null;
            }
            else
            {
                var responseData = _parkingRepository.GetParkedCarDetailsbyReceiptNo(receiptNo);
                return responseData;
            }
        }

        public ParkResponse GetParkedCarDetailsbyVehicleNo(string vehicleNo)
        {
            if (vehicleNo == null)
            {
                return null;
            }
            else
            {
                var responseData = _parkingRepository.GetParkedCarDetailsbyVehicleNo(vehicleNo);
                return responseData;
            }
        }
        public List<ParkResponse> ListOfParkedVehicle(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            else
            {
                var responseData = _parkingRepository.ListOfParkedVehicle(id);
                return responseData;
            }
        }

        public List<UnParkResponse> ListOfUnParkedVehicle(int id)
        {
            if (id <= 0)
            {
                return null;
            }
            else
            {
                var responseData = _parkingRepository.ListOfUnParkedVehicle(id);
                return responseData;
            }
        }

        public ParkResponse ParkVehicle(int securityID, ParkRequest parkDetails)
        {
            if (parkDetails != null)
            {
                var responseData = _parkingRepository.ParkVehicle(securityID, parkDetails);
                return responseData;
            }
            else
            {
                return null;
            }
        }

        public UnParkResponse UnParkVehicle(int securityID, int receiptNo)
        {
            if (receiptNo <= 0)
            {
                return null;
            }
            else
            {
                var responseData = _parkingRepository.UnParkVehicle(securityID, receiptNo);
                return responseData;
            }
        }
    }
}
