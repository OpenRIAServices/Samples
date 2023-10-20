using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using OpenRiaServices.Server;
using OpenRiaServices.Server.Authentication;

namespace AspNetCore.Hosting.AspNetCore.Services
{
    public class MyUser : IUser
    {
        [Key]
        public string Name { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }

    [EnableClientAccess]
    [Authorize]
    public class MyAuthenticationService : DomainService, IAuthentication<MyUser>
    {
        protected HttpContext HttpContext
          => base.ServiceContext.GetRequiredService<IHttpContextAccessor>().HttpContext!;

        [AllowAnonymous]
        public MyUser GetUser()
        {
            var user = (ClaimsPrincipal)base.ServiceContext.User;

            return (user.Identity?.IsAuthenticated == true)
                ? GetAuthenticatedUser(user)
                : GetAnonymousUser();
        }

        [AllowAnonymous]
        public async Task<MyUser> LoginAsync(string userName, string password, bool isPersistent, string customData)
        {
            if (ValidateCredentials(userName, password, customData))
            {
                // Maybe the claims is better extraced/created based on the "MyUser" type
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, userName),
                };

                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties()
                    {
                        IsPersistent = isPersistent,
                    });

                return GetAuthenticatedUser(principal);
            }
            else
            {
                // login failure
                return null;
            }
        }

        [AllowAnonymous]
        public async Task<MyUser> LogoutAsync()
        {
            await HttpContext.SignOutAsync();

            return GetAnonymousUser();
        }

        // [RequiresAuthentication] -- not needed since we have Authorize on class
        public void UpdateUser(MyUser user)
        {
            throw new System.NotImplementedException();
        }

        private MyUser GetAuthenticatedUser(ClaimsPrincipal principal)
        {
            return new MyUser()
            {
                Name = principal.Identity.Name,
                Roles = principal.FindAll(claim => claim.Type == ClaimTypes.Role).Select(claim => claim.Value).ToList(),
            };
        }

        private static MyUser GetAnonymousUser()
        {
            return new MyUser() { Name = string.Empty, Roles = Array.Empty<string>() };
        }

        private static bool ValidateCredentials(string userName, string password, string customData)
        {
            return password == "password";
        }

        MyUser IAuthentication<MyUser>.Login(string userName, string password, bool isPersistent, string customData)
            => throw new NotImplementedException("Async method is called at runtime");

        MyUser IAuthentication<MyUser>.Logout()
            => throw new NotImplementedException("Async method is called at runtime");
    }
}
