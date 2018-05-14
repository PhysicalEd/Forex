using System.Text.RegularExpressions;
using Contracts.Exceptions;
using Contracts.Validators;

namespace Logic.Validators
{
	public class EmailFormatValidator : IEmailFormatValidator
	{
		/// <summary>
		/// Indicates that this email does not already exist in the data store
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		public void Validate(string email)
		{
			if (string.IsNullOrEmpty(email)) return;

			// Check for other people with this email
			var regex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
			if (!regex.IsMatch(email)) { throw new EmailFormatException(); }
		}
	}
}
