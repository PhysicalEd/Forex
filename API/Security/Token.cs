using System;

namespace API.Security
{
    public class Token
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public DateTime WhenTokenExpires { get; set; }
    }
}