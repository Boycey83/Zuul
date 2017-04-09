using System;
using System.Security.Cryptography;
using System.Text;

namespace Zuul.BusinessLogic.Helpers
{
	public class AuthenticationHelper
	{
		private const int SaltLength = 16;
		private const int HashLength = 48;
		private const int PBKDF2Iterations = 1000;
		RNGCryptoServiceProvider _passwordSaltGenerator;

		public AuthenticationHelper()
		{
			_passwordSaltGenerator = new RNGCryptoServiceProvider();
		}

		public string GetPasswordSalt()
		{
			var saltBuffer = new byte[SaltLength];
			_passwordSaltGenerator.GetBytes(saltBuffer);
			return Convert.ToBase64String(saltBuffer);
		}

		public string GetPasswordHash(string password, string passwordSalt)
		{
			var saltedPasswordBytes = Encoding.UTF8.GetBytes(passwordSalt);
			var passwordHashGenerator = new Rfc2898DeriveBytes(password, saltedPasswordBytes, PBKDF2Iterations);
			var passwordHashBytes = passwordHashGenerator.GetBytes(HashLength);
			return Convert.ToBase64String(passwordHashBytes);
		}

		public bool ValidatePassword(string userAccountPasswordSalt, string userAccountPasswordHash, string passwordToValidate)
		{
			var hashedPasswordToValidate = GetPasswordHash(passwordToValidate, userAccountPasswordSalt);
			return hashedPasswordToValidate == userAccountPasswordHash;
		}
	}
}
