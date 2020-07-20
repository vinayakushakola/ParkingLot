using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotBusinessLayer.Interfaces
{
    public interface IPoliceBusiness
    {
        RegistrationResponse Registration(UserRegistrationRequest userDetails);

        RegistrationResponse Login(LoginRequest loginDetails);
    }
}
