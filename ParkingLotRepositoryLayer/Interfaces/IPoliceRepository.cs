using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotRepositoryLayer.Interfaces
{
    public interface IPoliceRepository
    {
        RegistrationResponse Registration(UserRegistrationRequest userDetails);

        RegistrationResponse Login(LoginRequest loginDetails);
    }
}
