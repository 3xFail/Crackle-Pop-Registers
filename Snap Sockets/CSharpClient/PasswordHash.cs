using System;
using System.Security.Cryptography;
using System.Text;

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

            return Convert.ToBase64String( hash );
        }

    }
}
