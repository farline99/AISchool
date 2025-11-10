using System.Security.Cryptography;

namespace AISchool
{
	public static class PasswordHasher
	{
		private const int SaltSize = 16;
		private const int KeySize = 32;
		private const int Iterations = 10000;
		private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;

		public static (string hash, string salt) HashPassword(string password)
		{
			var salt = RandomNumberGenerator.GetBytes(SaltSize);
			var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);
			return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
		}

		public static bool VerifyPassword(string password, string hash, string salt)
		{
			var saltBytes = Convert.FromBase64String(salt);
			var hashBytes = Convert.FromBase64String(hash);

			var newHash = Rfc2898DeriveBytes.Pbkdf2(password, saltBytes, Iterations, _hashAlgorithmName, KeySize);

			return CryptographicOperations.FixedTimeEquals(newHash, hashBytes);
		}
	}
}