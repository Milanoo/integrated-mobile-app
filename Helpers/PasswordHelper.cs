//--------------------------------------------------------------------
//   Date                 : November, 2023
//    Copyright: (C)2023 by AHT-GROUP, GmbH/SOFTWEL (P) Ltd
//    Email                : support at softwel dot com dot np 
// --------------------------------------------------------------------

// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License (GPL) as published by
// the Free Software Foundation (https://www.fsf.org/); either version 2
// of the License, or (at your option) any later version.

using System.Security.Cryptography;
using System.Text;


namespace IntegratedMobileApp.Helpers
{
	internal static class PasswordHelper
	{
		private static string CreateRandomSalt()
		{
			return Guid.NewGuid().ToString().Replace("{", "").Replace("}", "").Replace("-", "");
		}

		public static string CreateHashedPwd(string pwd)
		{
			var salt = CreateRandomSalt();
			var computedHash = ComputeSha256Hash(pwd + salt);
			return salt + ":" + computedHash;
		}

		public static bool VerifyHashedPassword(string pwd, string saltedPwd)
		{
			var p = saltedPwd.Split(':');
			var salt = p[0];
			var hashedPwd = p[1];

			var computedHash = ComputeSha256Hash(pwd + salt);
			return (computedHash == hashedPwd);
		}

		private static string ComputeSha256Hash(string rawData)
		{
			// Create a SHA256   
			using (SHA256 sha256Hash = SHA256.Create())
			{
				// ComputeHash - returns byte array  
				byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

				// Convert byte array to a string   
				StringBuilder builder = new StringBuilder();

				foreach (var t in bytes)
				{
					builder.Append(t.ToString("x2"));
				}
				return builder.ToString();
			}
				
		}
	}
}
