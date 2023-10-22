using BusinessLogicLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Helpers
{
	public class EncryptionHelper
	{
		public static string CreatePasswordHash(string password, string saltKey, HashedPasswordFormat passwordFormat = HashedPasswordFormat.SHA512)
		{
			var saltAndPassword = string.Concat(password, saltKey);
			HashAlgorithm algorithm = passwordFormat switch
			{
				HashedPasswordFormat.SHA1 => SHA1.Create(),
				HashedPasswordFormat.SHA256 => SHA256.Create(),
				HashedPasswordFormat.SHA384 => SHA384.Create(),
				HashedPasswordFormat.SHA512 => SHA512.Create(),
				_ => throw new NotSupportedException("Not supported format")
			};
			if (algorithm == null)
				throw new ArgumentException("Unrecognized hash name");

			var hashByteArray = algorithm.ComputeHash(Encoding.UTF8.GetBytes(saltAndPassword));
			return BitConverter.ToString(hashByteArray).Replace("-", "");
		}
	}
}
