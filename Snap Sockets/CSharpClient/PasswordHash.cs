using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CSharpClient
{
    public sealed class PasswordHash
    {

        private static readonly int HashBytes = 20;
        public static string Hash( string username, string pass )
        {

            byte[] salt = Encoding.ASCII.GetBytes( username );
            var pbkdf2 = new Rfc2898DeriveBytes( pass, salt, 10000 );

            byte[] hash = pbkdf2.GetBytes( HashBytes );
            byte[] hashresult = new byte[username.Length + HashBytes];

            Array.Copy( salt, 0, hashresult, 0, username.Length );
            Array.Copy( hash, 0, hashresult, username.Length, HashBytes );

            return Convert.ToBase64String( hashresult );
        }

    }
}
