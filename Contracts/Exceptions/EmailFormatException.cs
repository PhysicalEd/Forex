namespace Contracts.Exceptions
{
	public class EmailFormatException : UserException
	{
		public EmailFormatException() : base("Your email is not a valid format") { }
	}
}