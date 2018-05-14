using System;
using System.Collections.Generic;
using Contracts.Security;

namespace Logic.Security
{
	public class PasswordGenerator : IPasswordGenerator
	{
		/// <summary>
		/// Generates this password info
		/// </summary>
		/// <returns></returns>
		public string Generate()
		{
			return this.CreateRandomPassword();
		}

		/// <summary>
		/// Creates a random password
		/// </summary>
		/// <returns></returns>
		private string CreateRandomPassword()
		{
			var exclude = new List<char>() {'a', 'e', 'i', 'o', 'u'};
			string password = "";
			var ran = new Random(DateTime.Now.Millisecond);
			while (password.Length < 6)
			{
				var ascii = ran.Next(97, 123);
				char c = (char) ascii;
				if (!exclude.Contains(c)) password += c;
			}

			return password;
		}
	}
}
