using ParkingLotBusinessLayer.Interfaces;
using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;
using ParkingLotRepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotBusinessLayer.Services
{
    public class AdminBusiness : IAdminBusiness
    {
        private readonly IAdminRepository _adminRepository;

        public AdminBusiness(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public List<RegistrationResponse> ListOfSecurity(int adminID)
        {
            if (adminID <= 0)
                return null;
            else
                return _adminRepository.ListOfSecurity(adminID);
        }

        public RegistrationResponse Registration(AdminRegistrationRequest adminDetails)
        {
            if (adminDetails == null)
                return null;
            else
                return _adminRepository.Registration(adminDetails);
        }

        public RegistrationResponse Login(LoginRequest loginDetails)
        {
            if (loginDetails == null)
                return null;
            else
                return _adminRepository.Login(loginDetails);
        }
    }
}
