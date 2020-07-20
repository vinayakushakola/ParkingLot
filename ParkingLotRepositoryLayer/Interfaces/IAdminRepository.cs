using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotRepositoryLayer.Interfaces
{
    public interface IAdminRepository
    {
        List<RegistrationResponse> ListOfSecurity(int adminID);

        RegistrationResponse Registration(AdminRegistrationRequest adminDetails);

        RegistrationResponse Login(LoginRequest loginDetails);
    }
}
