using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Contracts.DataManagers;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace API.Security
{
    public class OAuthAppProvider : OAuthAuthorizationServerProvider
    {
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                var username = context.UserName;
                var password = context.Password;

                var accountMgr = Dependency.Dependency.Resolve<IAccountManager>();
                var login = accountMgr.SignIn(username, password);

                //var login = Dependency.Resolve<IPersonManager>().GetTestLogin();
                if (login != null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, login.FirstName),
                        new Claim("UserID", login.LoginID.ToString())
                    };

                
                    ClaimsIdentity oAutIdentity = new ClaimsIdentity(claims, Startup.OAuthOptions.AuthenticationType);
                    context.Validated(new AuthenticationTicket(oAutIdentity, new AuthenticationProperties() { }));
                }
                else
                {
                    context.SetError("invalid_grant", "Error");
                }
            });
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }
    }
}
