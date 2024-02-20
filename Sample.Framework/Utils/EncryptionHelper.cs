using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Framework.Utils
{
    public class EncryptionHelper
    {
        public static string HashedPassword(string password)
        {

            string salt = BCrypt.Net.BCrypt.GenerateSalt(12); // The recommended number of rounds is 12
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }
    }
}
