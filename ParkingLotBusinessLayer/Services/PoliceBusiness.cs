using ParkingLotBusinessLayer.Interfaces;
using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;
using ParkingLotRepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotBusinessLayer.Services
{
    public class PoliceBusiness : IPoliceBusiness
    {
        private readonly IPoliceRepository _userRepository;

        public PoliceBusiness(IPoliceRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public RegistrationResponse Registration(UserRegistrationRequest userDetails)
        {
            if (userDetails == null)
                return null;
            else
                return _userRepository.Registration(userDetails);
        }

        public RegistrationResponse Login(LoginRequest loginDetails)
        {
            if (loginDetails == null)
                return null;
            else
                return _userRepository.Login(loginDetails);
        }
    }
}
