using ParkingLotCommonLayer.CommonClasses;
using ParkingLotCommonLayer.ModelDB;
using ParkingLotCommonLayer.RequestModels;
using ParkingLotCommonLayer.ResponseModels;
using ParkingLotRepositoryLayer.ApplicationContext;
using ParkingLotRepositoryLayer.Interfaces;
using System;
using System.Linq;

namespace ParkingLotRepositoryLayer.Services
{
    public class SecurityRepository : ISecurityRepository
    {
        private readonly AppDBContext _appDBContext;
        private static readonly string _user = "Security";

        public SecurityRepository(AppDBContext appDBContext)
        {
            _appDBContext = appDBContext;
        }

        public RegistrationResponse Registration(UserRegistrationRequest userDetails)
        {
            try
            {
                RegistrationResponse responseData = null;
                userDetails.Password = EncodeDecode.EncodePasswordToBase64(userDetails.Password);
                var userData = new Users
                {
                    FirstName = userDetails.FirstName,
                    LastName = userDetails.LastName,
                    Email = userDetails.Email,
                    Password = userDetails.Password,
                    IsActive = true,
                    UserRole = _user,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now
                };

                _appDBContext.Add(userData);
                _appDBContext.SaveChanges();

                responseData = ResponseData(userData);
                return responseData;
            }
            catch(Exception ex)
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

                var userData = _appDBContext.Users
                    .Where(user => user.Email == loginDetails.Email && user.Password == loginDetails.Password)
                    .FirstOrDefault();

                if (userData != null)
                {
                    responseData = ResponseData(userData);
                }
                return responseData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static RegistrationResponse ResponseData(Users user)
        {
            try
            {
                RegistrationResponse responseData = new RegistrationResponse
                {
                    ID = user.ID,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    UserRole = user.UserRole,
                    CreatedDate = user.CreatedDate,
                    ModifiedDate = user.ModifiedDate
                };
                return responseData;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
