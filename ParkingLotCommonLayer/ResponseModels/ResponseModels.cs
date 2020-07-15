using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotCommonLayer.ResponseModels
{
    public class RegistrationResponse
    {
        public int ID { get; set; }

        public string FirstName { set; get; }

        public string LastName { set; get; }

        public string Email { set; get; }

        public bool IsActive { set; get; }

        public string UserRole { set; get; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
