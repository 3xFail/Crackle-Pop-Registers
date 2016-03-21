using System;
using System.Security.Cryptography;
using System.Text;

namespace CSharpClient
{
    public sealed class PasswordHash
    {

        public static string HashPassword( string password )
        {
            return BCrypt.Net.BCrypt.HashPassword( password, 12 );
        }

        public static bool ValidatePassword( string password, string correctHash )
        {
            return BCrypt.Net.BCrypt.Verify( password, correctHash );
        }
    }

}
