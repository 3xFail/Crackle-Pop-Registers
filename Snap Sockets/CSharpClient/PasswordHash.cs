using System;
using System.Security.Cryptography;
using System.Text;

namespace CSharpClient
{
    //https://cmatskas.com/-net-password-hashing-using-pbkdf2/
    public sealed class PasswordHash
    {
        private static string GetRandomSalt()
        {
            return BCrypt.Net.BCrypt.GenerateSalt( 12 );
        }

        public static string HashPassword( string password )
        {
            return BCrypt.Net.BCrypt.HashPassword( password, GetRandomSalt() );
        }

        public static bool ValidatePassword( string password, string correctHash )
        {
            return BCrypt.Net.BCrypt.Verify( password, correctHash );
        }
    }

}
