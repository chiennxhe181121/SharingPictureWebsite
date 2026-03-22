using System.Security.Cryptography;

namespace SharingPictureWebsite.Service.Security
{
    public static class AppPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        public static string HashPassword(string password)
        {
            var salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            var key = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                KeySize);

            return $"PBKDF2${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(key)}";
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            try
            {
                var parts = hashedPassword.Split('$');
                if (parts.Length != 4 || parts[0] != "PBKDF2")
                    return false;

                var iterations = int.Parse(parts[1]);
                var salt = Convert.FromBase64String(parts[2]);
                var key = Convert.FromBase64String(parts[3]);

                var keyToCheck = Rfc2898DeriveBytes.Pbkdf2(
                    password,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256,
                    key.Length);

                return CryptographicOperations.FixedTimeEquals(key, keyToCheck);
            }
            catch
            {
                return false;
            }
        }
    }
}
