using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizWhois.Domain.Entity;

namespace QuizWhois.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private List<User> Users = null;

        public AuthController()
        {
            Users = new List<User>();
            Users.Add(new User()
            {
                Id = 1,
                Login = "test",
                Password = "123",
            });
        }       

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel objLoginModel)
        {
            if (ModelState.IsValid)
            {
                var user = Users.Where(x => x.Login == objLoginModel.UserName && x.Password == objLoginModel.Password).FirstOrDefault();
                if (user == null)
                {
                    ViewBag.Message = "Invalid Credential";
                    return Ok("Not Ok!");
                }
                else
                {
                    //var claims = new List<Claim>()
                    //{
                    //    new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.UserId)),
                    //    new Claim(ClaimTypes.Name, user.Username),
                    //    new Claim(ClaimTypes.Role, user.Role),
                    //    new Claim("FavoriteDrink", "Tea")
                    //};
                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties()
                    {
                        IsPersistent = objLoginModel.RememberLogin
                    });
                }
            }

            return Ok(User.Identity.Name);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok("Success LogOut");
        }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }

        public string ReturnUrl { get; set; }
    }    
}
