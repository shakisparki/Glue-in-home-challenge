using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TT.Deliveries.Auth.Services.Contracts;

namespace TT.Deliveries.Auth.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthSchemeOptions>
    {
        private readonly IAuthService _authService;
        public BasicAuthenticationHandler(
            IAuthService authService,
            IOptionsMonitor<BasicAuthSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) :
           base(options, logger, encoder, clock)
        {
            _authService = authService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                Logger.LogError("Request Missing Authorization Header");
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

            if (authHeader.Scheme != "Basic")
            {
                return AuthenticateResult.Fail("Invalid Authorization Scheme");
            }

            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            var username = credentials[0];
            var password = credentials[1];
            var user = _authService.Authenticate(username, password);
            if (user is not null)
            {
                var claims = user.Claims.Select(x => new Claim(ClaimTypes.Role, x)).ToList();
                claims.Add(new Claim(ClaimTypes.Name, username));
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                return AuthenticateResult.Success(ticket);
            }
            else
            {
                Logger.LogError("Invalid Username or Password");
                return AuthenticateResult.Fail("Invalid Username or Password");
            }
        }
       
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.Headers["WWW-Authenticate"] = $"{Scheme.Name} realm=\"{Options.Realm}\", charset=\"UTF-8\"";
            await base.HandleChallengeAsync(properties);
        }
    }
}
