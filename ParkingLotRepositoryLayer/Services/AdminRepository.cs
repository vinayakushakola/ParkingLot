using ParkingLotCommonLayer.CommonClasses;
using ParkingLotCommonLayer.ModelDB;
using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;
using ParkingLotRepositoryLayer.ApplicationContext;
using ParkingLotRepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParkingLotRepositoryLayer.Services
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDBContext _appDBContext;
        private static readonly string _admin = "Admin";
        private static readonly string _user = "User";

        public AdminRepository(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public List<RegistrationResponse> ListOfUsers(int adminID)
        {
            try
            {
                List<RegistrationResponse> listOfResponseData = null;
                var adminData = _appDBContext.Users
                    .Where(admin => admin.ID == adminID && admin.UserRole == _admin)
                    .FirstOrDefault();

                if (adminData != null)
                {
                    listOfResponseData = _appDBContext.Users
                        .Where(user => user.UserRole == _user)
                        .Select(user => new RegistrationResponse
                        {
                            ID = user.ID,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            IsActive = user.IsActive,
                            UserRole = user.UserRole,
                            CreatedDate = user.CreatedDate,
                            ModifiedDate = user.ModifiedDate
                        }).ToList();
                }
                return listOfResponseData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public RegistrationResponse Registration(AdminRegistrationRequest adminDetails)
        {
            try
            {
                RegistrationResponse responseData = null;
                adminDetails.Password = EncodeDecode.EncodePasswordToBase64(adminDetails.Password);
                var adminData = new Users
                {
                    FirstName = adminDetails.FirstName,
                    LastName = adminDetails.LastName,
                    Email = adminDetails.Email,
                    Password = adminDetails.Password,
                    IsActive = true,
                    UserRole = "Admin",
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                _appDBContext.Add(adminData);
                _appDBContext.SaveChanges();

                responseData = SecurityRepository.ResponseData(adminData);
                return responseData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public RegistrationResponse Login(LoginRequest loginDetails)
        {
            try
            {
                RegistrationResponse responseData = null;
                loginDetails.Password = EncodeDecode.EncodePasswordToBase64(loginDetails.Password);

                var adminData = _appDBContext.Users
                    .Where(user => user.Email == loginDetails.Email && user.Password == loginDetails.Password)
                    .FirstOrDefault();

                if (adminData != null)
                {
                    responseData = SecurityRepository.ResponseData(adminData);
                }
                return responseData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
