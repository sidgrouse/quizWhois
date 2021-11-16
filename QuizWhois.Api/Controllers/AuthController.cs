using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Google.Apis.Auth.AspNetCore3;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using QuizWhois.Common.Models;
using QuizWhois.Domain.Entity;

namespace QuizWhois.Api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {        
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Success LogOut");
        }

        [Authorize]
        [HttpGet("login")]
        public IActionResult LogIn()
        {           
            return Ok("Success LogIn");
        }

        [Authorize]
        [HttpGet("scopes")]
        public async Task<ActionResult<List<string>>> ScopeListing([FromServices] IGoogleAuthProvider auth)
        {
            IEnumerable<string> currentScopes = await auth.GetCurrentScopesAsync();
            return currentScopes.ToList();
        }

        [Authorize]
        [HttpGet("tokens")]
        public async Task<ActionResult<string>> ShowTokens()
        {
            // The user is already authenticated, so this call won't trigger authentication.
            // But it allows us to access the AuthenticateResult object that we can inspect
            // to obtain token related values.
            AuthenticateResult authResult = await HttpContext.AuthenticateAsync();
            string idToken = authResult.Properties.GetTokenValue(OpenIdConnectParameterNames.IdToken);
            string idTokenValid, idTokenIssued, idTokenExpires;
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
                idTokenValid = "true";
                idTokenIssued = DateTimeOffset.FromUnixTimeSeconds(payload.IssuedAtTimeSeconds.Value).DateTime.ToString();
                idTokenExpires = DateTimeOffset.FromUnixTimeSeconds(payload.ExpirationTimeSeconds.Value).DateTime.ToString();
            }
            catch (Exception e)
            {
                idTokenValid = $"false: {e.Message}";
                idTokenIssued = "invalid";
                idTokenExpires = "invalid";
            }

            string accessToken = authResult.Properties.GetTokenValue(OpenIdConnectParameterNames.AccessToken);
            string refreshToken = authResult.Properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);
            string accessTokenExpiresAt = authResult.Properties.GetTokenValue("expires_at");
            string cookieIssuedUtc = authResult.Properties.IssuedUtc?.ToString() ?? "<missing>";
            string cookieExpiresUtc = authResult.Properties.ExpiresUtc?.ToString() ?? "<missing>";

            var result = new StringBuilder();
            result.AppendLine($"Id Token: '{idToken}'");
            result.AppendLine($"Id Token valid: {idTokenValid}");
            result.AppendLine($"Id Token Issued UTC: '{idTokenIssued}'");
            result.AppendLine($"Id Token Expires UTC: '{idTokenExpires}'");
            result.AppendLine($"Access Token: '{accessToken}'");
            result.AppendLine($"Refresh Token: '{refreshToken}'");
            result.AppendLine($"Access token expires at: '{accessTokenExpiresAt}'");
            result.AppendLine($"Cookie Issued UTC: '{cookieIssuedUtc}'");
            result.AppendLine($"Cookie Expires UTC: '{cookieExpiresUtc}'");
            return result.ToString();            
        }

        public class ForceTokenRefreshModel
        {
            public IReadOnlyList<string> Results { get; set; }

            public string AccessToken { get; set; }
        }

        /// <summary>
        /// Forces the refresh of the OAuth access token.
        /// Specifying the <see cref="AuthorizeAttribute"/> will guarantee that the code executes only if the
        /// user is authenticated.
        /// </summary>
        /// <param name="auth">The Google authorization provider.
        /// This can also be injected on the controller constructor.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Authorize]
        [HttpGet("forceTokenRefresh")]
        public async Task<ActionResult<ForceTokenRefreshModel>> ForceTokenRefresh([FromServices] IGoogleAuthProvider auth)
        {
            // Obtain OAuth related values before the refresh.
            AuthenticateResult authResult0 = await HttpContext.AuthenticateAsync();
            string accessToken0 = authResult0.Properties.GetTokenValue(OpenIdConnectParameterNames.AccessToken);
            string refreshToken0 = authResult0.Properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);
            string issuedUtc0 = authResult0.Properties.IssuedUtc?.ToString() ?? "<missing>";
            string expiresUtc0 = authResult0.Properties.ExpiresUtc?.ToString() ?? "<missing>";

            // Force token refresh by specifying a too-long refresh window.
            GoogleCredential cred = await auth.GetCredentialAsync(TimeSpan.FromHours(24));

            // Obtain OAuth related values after the refresh.
            AuthenticateResult authResult1 = await HttpContext.AuthenticateAsync();
            string accessToken1 = authResult1.Properties.GetTokenValue(OpenIdConnectParameterNames.AccessToken);
            string refreshToken1 = authResult1.Properties.GetTokenValue(OpenIdConnectParameterNames.RefreshToken);
            string issuedUtc1 = authResult1.Properties.IssuedUtc?.ToString() ?? "<missing>";
            string expiresUtc1 = authResult1.Properties.ExpiresUtc?.ToString() ?? "<missing>";

            // As demonstration compare the old values with the new ones and check that everything is
            // as it should be.
            string credAccessToken = await cred.UnderlyingCredential.GetAccessTokenForRequestAsync();

            bool accessTokenChanged = accessToken0 != accessToken1;
            bool credHasCorrectAccessToken = credAccessToken == accessToken1;

            bool pass = accessTokenChanged && credHasCorrectAccessToken;

            var model = new ForceTokenRefreshModel
            {
                Results = new[]
                {
                    $"Before Access Token: '{accessToken0}'",
                    $"Before Refresh Token: '{refreshToken0}'",
                    $"Before Issued UTC: '{issuedUtc0}'",
                    $"Before Expires UTC: '{expiresUtc0}'",
                    $"After Access Token: '{accessToken1}'",
                    $"After Refresh Token: '{refreshToken1}'",
                    $"After Issued UTC: '{issuedUtc1}'",
                    $"After Expires UTC: '{expiresUtc1}'",
                    $"Access token changed: {accessTokenChanged}",
                    $"Credential has correct access token: {credHasCorrectAccessToken}",
                    $"Pass: {pass}"
                },
                AccessToken = accessToken1
            };
            return model;
        }

        /// <summary>
        /// Checks that the access token is the expected one.
        /// Specifying the <see cref="AuthorizeAttribute"/> will guarantee that the code executes only if the
        /// user is authenticated.
        /// This method is used from the Force Refresh sample to show that the refreshed token is persisted.
        /// </summary>
        /// <param name="auth">The Google authorization provider.
        /// This can also be injected on the controller constructor.</param>
        /// <param name="expectedAccessToken">The expected token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Authorize]
        [HttpPost("forceTokenRefreshCheckPersisted")]
        public async Task<ActionResult<string>> ForceTokenRefreshCheckPersisted([FromServices] IGoogleAuthProvider auth, [FromForm] string expectedAccessToken)
        {
            // Check that the refreshed access token is correctly persisted across requests.
            var cred = await auth.GetCredentialAsync();
            var credAccessToken = await cred.UnderlyingCredential.GetAccessTokenForRequestAsync();
            var pass = credAccessToken == expectedAccessToken;
            var result = new StringBuilder();
            result.AppendLine($"Expected access token: '{expectedAccessToken}'");
            result.AppendLine($"Credential access token: '{credAccessToken}'");
            result.AppendLine($"Pass: {pass}");
            return result.ToString();            
        }
    }
}
