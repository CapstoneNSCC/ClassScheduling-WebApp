using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ClassScheduling_WebApp.Models
{

    [Table("TblUser")]
    public class UserModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; }

        [Required]
        public bool SetAsAdmin { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(30, ErrorMessage = "Username cannot exceed 30 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }

        public string Salt { get; set; }

        // Collection navigation properties
        public virtual ICollection<CourseModel> Courses { get; set; } = new List<CourseModel>();

        public string GetHashed(string password, string salt)
        {
            byte[] saltBytes = Convert.FromBase64String(salt);
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: saltBytes,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashed;
        }

        public string getSalt()
        {
            // generate a 128-bit salt using a secure PRNG (pseudo-random number generator)
            // 128 / 8 = 16 bytes = 128 bits
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            //Console.WriteLine(">>> Salt: " + Convert.ToBase64String(salt));

            return Convert.ToBase64String(salt);
        }
    }

}
