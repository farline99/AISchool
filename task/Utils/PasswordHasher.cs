using System.Text;

namespace AISchool
{
    public static class PasswordHasher
    {
        public static (string hash, string salt) HashPassword(string password)
        {
            string bcryptHash = BCrypt.Net.BCrypt.HashPassword(password);

            byte[] hashBytes = Encoding.UTF8.GetBytes(bcryptHash);

            return (Convert.ToBase64String(hashBytes), "");
        }

        public static bool VerifyPassword(string password, string hashBase64, string saltBase64Ignored)
        {
            try
            {
                if (string.IsNullOrEmpty(hashBase64)) return false;

                byte[] hashBytes = Convert.FromBase64String(hashBase64);
                string bcryptHash = Encoding.UTF8.GetString(hashBytes);

                return BCrypt.Net.BCrypt.Verify(password, bcryptHash);
            }
            catch
            {
                return false;
            }
        }
    }
}