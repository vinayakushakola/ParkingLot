using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingLotCommonLayer.CommonClasses
{
    public class EncodeDecode
    {

        /// <summary>
        /// It Encrpyt the Password to Base64
        /// </summary>
        /// <param name="password">User Password</param>
        /// <returns>Encrypted Password</returns>
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
    }
}
