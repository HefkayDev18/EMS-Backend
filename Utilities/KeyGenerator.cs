using System.Security.Cryptography;

namespace EmployeeManagementSystem.Utilities
{
    public class KeyGenerator
    {
        public string GenerateKey()
        {
            byte[] randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            string key = Convert.ToBase64String(randomBytes);

            return key;
        }
    }
}
