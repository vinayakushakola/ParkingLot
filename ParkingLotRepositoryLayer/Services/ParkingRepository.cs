using ParkingLotCommonLayer.CommonClasses;
using ParkingLotCommonLayer.ModelDB;
using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;
using ParkingLotRepositoryLayer.ApplicationContext;
using ParkingLotRepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

namespace ParkingLotRepositoryLayer.Services
{
    public class ParkingRepository : IParkingRepository
    {
        private readonly AppDBContext _appDBContext;
        private static string parkingSlot;

        public ParkingRepository(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public ParkingSlotResponse AvailableParkingSlots()
        {
            try
            {
                ParkingSlotResponse responseData = new ParkingSlotResponse();
                ParkingSlots parkingSlots = null;
                var slotACount = _appDBContext.ParkingDetails.Where(parkingDetails => parkingDetails.ParkingSlot == "A" && parkingDetails.IsParked == true).Count();
                var slotBCount = _appDBContext.ParkingDetails.Where(parkingDetails => parkingDetails.ParkingSlot == "B" && parkingDetails.IsParked == true).Count();
                var slotCCount = _appDBContext.ParkingDetails.Where(parkingDetails => parkingDetails.ParkingSlot == "C" && parkingDetails.IsParked == true).Count();
                int aCount = ParkingLimit.Parking_Slot_A - slotACount;
                int bCount = ParkingLimit.Parking_Slot_B - slotBCount;
                int cCount = ParkingLimit.Parking_Slot_B - slotCCount;
                parkingSlots = new ParkingSlots()
                {
                    Parking_Slot_A = aCount,
                    Parking_Slot_B = bCount,
                    Parking_Slot_C = cCount
                };
                responseData.ParkingSlots = parkingSlots;
                return responseData;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ParkResponse ParkVehicle(int securityID, ParkRequest parkDetails)
        {
            try
            {
                var securityExists = _appDBContext.Users.
                    Any(security => security.ID == securityID);
                if (securityExists)
                {
                    ParkingDetails parkData = new ParkingDetails
                    {
                        OwnerName = parkDetails.OwnerName,
                        VehicleNumber = parkDetails.VehicleNumber,
                        VehicleBrand = parkDetails.VehicleBrand,
                        Color = parkDetails.Color,
                        IsParked = true,
                        AttendantName = parkDetails.AttendantName,
                        Disability = parkDetails.Disability,
                        ParkingDate = DateTime.Now
                    };
                    if (parkDetails.Disability)
                        parkingSlot = GetParkingSlotForHandicap();
                    else
                        parkingSlot = GetParkingSlot();
                    if (parkingSlot == null)
                    {
                        return null;
                    }
                    parkData.ParkingSlot = parkingSlot;
                    _appDBContext.ParkingDetails.Add(parkData);
                    _appDBContext.SaveChanges();
                    ParkResponse responseData = new ParkResponse
                    {
                        ReceiptNumber = parkData.ReceiptNumber,
                        OwnerName = parkData.OwnerName,
                        VehicleNumber = parkData.VehicleNumber,
                        VehicleBrand = parkData.VehicleBrand,
                        Color = parkData.Color,
                        Disability = parkData.Disability,
                        AttendantName = parkData.AttendantName,
                        ParkingDate = parkData.ParkingDate,
                        ParkingSlot = parkData.ParkingSlot
                    };
                    return responseData;
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public UnParkResponse UnParkVehicle(int securityID, int receiptNo)
        {
            try
            {
                var securityExists = _appDBContext.Users.
                    Any(security => security.ID == securityID);
                if (securityExists)
                {
                    double total = _appDBContext.ParkingDetails.
                        Where(vehicle => vehicle.ReceiptNumber == receiptNo).
                        Select(vehicle => (DateTime.Now.Subtract(vehicle.ParkingDate).TotalMinutes)).Sum();

                    UnParkedDetails unParkedData = new UnParkedDetails
                    {
                        ReceiptNumber = receiptNo,
                        UnParkedDate = DateTime.Now,
                        TotalTime = total,
                        TotalAmt = total * 10
                    };
                    _appDBContext.UnParkedDetails.Add(unParkedData);
                    _appDBContext.SaveChanges();

                    var unParkData = _appDBContext.ParkingDetails.
                        Where(vehicle => vehicle.ReceiptNumber == receiptNo).
                        FirstOrDefault();
                    unParkData.IsParked = false;
                    _appDBContext.SaveChanges();

                    UnParkResponse responseData = new UnParkResponse
                    {
                        ReceiptNumber = unParkData.ReceiptNumber,
                        OwnerName = unParkData.OwnerName,
                        VehicleNumber = unParkData.VehicleNumber,
                        VehicleBrand = unParkData.VehicleBrand,
                        Color = unParkData.Color,
                        IsParked = unParkData.IsParked,
                        Disability = unParkData.Disability,
                        AttendantName = unParkData.AttendantName,
                        ParkingDate = unParkData.ParkingDate,
                        UnParkedDate = unParkedData.UnParkedDate,
                        ParkingSlot = unParkData.ParkingSlot,
                        TotalTime = unParkedData.TotalTime,
                        TotalAmt = unParkedData.TotalAmt
                    };
                    return responseData;
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ParkResponse> ListOfParkedVehicle(int id)
        {
            try
            {
                List<ParkResponse> responseData = null;
                var Exists = _appDBContext.Users.
                    Any(security => security.ID == id);
                if (Exists)
                {
                    responseData = _appDBContext.ParkingDetails.
                        Where(vehicle => vehicle.ReceiptNumber > 0 && vehicle.IsParked == true).
                        Select(vehicle => new ParkResponse
                        {
                            ReceiptNumber = vehicle.ReceiptNumber,
                            OwnerName = vehicle.OwnerName,
                            VehicleNumber = vehicle.VehicleNumber,
                            VehicleBrand = vehicle.VehicleBrand,
                            Color = vehicle.Color,
                            Disability = vehicle.Disability,
                            AttendantName = vehicle.AttendantName,
                            ParkingDate = vehicle.ParkingDate,
                            ParkingSlot = vehicle.ParkingSlot
                        }).ToList();
                }
                if (responseData != null)
                    return responseData;
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<UnParkResponse> ListOfUnParkedVehicle(int id)
        {
            try
            {
                List<UnParkResponse> unParkedVehicleData = null;
                var Exists = _appDBContext.Users.
                    Any(security => security.ID == id);
                if (Exists)
                {
                    unParkedVehicleData = _appDBContext.ParkingDetails.
                        Where(vehicle => vehicle.ReceiptNumber > 0 && vehicle.IsParked == false).
                        Join(_appDBContext.UnParkedDetails,
                        park => park.ReceiptNumber,
                        unPark => unPark.ReceiptNumber,
                        (park, unPark) => new UnParkResponse
                        {
                            ReceiptNumber = park.ReceiptNumber,
                            OwnerName = park.OwnerName,
                            VehicleNumber = park.VehicleNumber,
                            VehicleBrand = park.VehicleBrand,
                            Color = park.Color,
                            IsParked = park.IsParked,
                            Disability = park.Disability,
                            AttendantName = park.AttendantName,
                            ParkingDate = park.ParkingDate,
                            UnParkedDate = unPark.UnParkedDate,
                            ParkingSlot = park.ParkingSlot,
                            TotalTime = unPark.TotalTime,
                            TotalAmt = unPark.TotalAmt
                        }).ToList();
                }
                if (unParkedVehicleData != null)
                    return unParkedVehicleData;
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public string GetParkingSlotForHandicap()
        {

            var slotACount = _appDBContext.ParkingDetails.Where(parkingDetails => parkingDetails.ParkingSlot == "A" && parkingDetails.IsParked == true).Count();
            var slotBCount = _appDBContext.ParkingDetails.Where(parkingDetails => parkingDetails.ParkingSlot == "B" && parkingDetails.IsParked == true).Count();
            var slotCCount = _appDBContext.ParkingDetails.Where(parkingDetails => parkingDetails.ParkingSlot == "C" && parkingDetails.IsParked == true).Count();

            if (slotACount < ParkingLimit.Parking_Slot_A)
            {
                return "A";
            }
            else if (slotBCount < ParkingLimit.Parking_Slot_B)
            {
                return "B";
            }
            else if (slotCCount < ParkingLimit.Parking_Slot_C)
            {
                return "C";
            }
            return null;
        }

        public string GetParkingSlot()
        {

            var slotACount = _appDBContext.ParkingDetails.Where(parkingDetails => parkingDetails.ParkingSlot == "A" && parkingDetails.IsParked == true).Count();
            var slotBCount = _appDBContext.ParkingDetails.Where(parkingDetails => parkingDetails.ParkingSlot == "B" && parkingDetails.IsParked == true).Count();
            var slotCCount = _appDBContext.ParkingDetails.Where(parkingDetails => parkingDetails.ParkingSlot == "C" && parkingDetails.IsParked == true).Count();

            if (slotCCount < ParkingLimit.Parking_Slot_C)
            {
                return "C";
            }
            else if (slotBCount < ParkingLimit.Parking_Slot_B)
            {
                return "B";
            }
            else if (slotACount < ParkingLimit.Parking_Slot_A)
            {
                return "A";
            }
            return null;
        }
    }
}
