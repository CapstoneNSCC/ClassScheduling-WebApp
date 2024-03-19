using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using ClassScheduling_WebApp.Data;
using System.Linq;

namespace ClassScheduling_WebApp.Models
{
    public class WebLogin
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpContext _httpContext;
        public string Username { get; set; }
        public string Password { get; set; }
        public bool Access { get; private set; }

        public WebLogin(ApplicationDbContext context, HttpContext httpContext)
        {
            _context = context;
            _httpContext = httpContext;
            _httpContext.Session.Clear();
            Username = "";
            Password = "";
            Access = false;
        }

        public string username
        {
            set
            {
                Username = (value == null ? "" : value);
            }
        }

        public string password
        {
            set
            {
                Password = (value == null ? "" : value);
            }
        }

        public bool access
        {
            get { return Access; }
        }

        public bool Unlock()
        {

            //trim to 10 characters in case front end maxLength compromised
            Username = truncate(Username, 10);
            Password = truncate(Password, 10);

            var user = _context.Users.SingleOrDefault(u => u.UserName == Username);
            if (user != null)
            {
                var hashedPassword = GetHashed(Password, user.Salt.ToString());
                if (hashedPassword == user.Password)
                {
                    Access = true;
                    _httpContext.Session.SetString("auth", "true");
                    _httpContext.Session.SetString("user", Username);
                    return true;
                }
            }
            return false;
        }

        private string GetHashed(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            Console.WriteLine("----- teste ----- ");
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
                Console.WriteLine("hashed: " + hashed);
            return hashed;
        }
        private string truncate(string value, int maxLength)
        {
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
