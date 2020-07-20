using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotBusinessLayer.Interfaces
{
    public interface IAdminBusiness
    {
        List<RegistrationResponse> ListOfSecurity(int adminID);

        RegistrationResponse Registration(AdminRegistrationRequest adminDetails);

        RegistrationResponse Login(LoginRequest loginDetails);
    }
}
