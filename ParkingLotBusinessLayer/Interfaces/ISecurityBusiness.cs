using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotBusinessLayer.Interfaces
{
    public interface ISecurityBusiness
    {
        RegistrationResponse Registration(UserRegistrationRequest userDetails);

        RegistrationResponse Login(LoginRequest loginDetails);
    }
}
