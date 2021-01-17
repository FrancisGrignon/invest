using Invest.MVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Invest.MVC.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            var model = new LoginViewModel();

            model.ReturnUrl = returnUrl;

            return View(model);
        }

        // POST: Login
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([FromForm] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_config["AdminPassword"].Equals(model.Password))
                    {
                        await SignInUser(model.Username, true);

                        if (string.IsNullOrEmpty(model.ReturnUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }

                        return Redirect(model.ReturnUrl);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);

                    return View(model);
                }
            }

            ModelState.AddModelError(string.Empty, "Courriel ou téléphone ou mot de passe invalide.");

            return View(model);
        }

        public ActionResult Logout()
        {
            return View();
        }

        /// <summary>  
        /// Sign In User method.  
        /// </summary>  
        /// <param name="username">Username parameter.</param>  
        /// <param name="isPersistent">Is persistent parameter.</param>  
        /// <returns>Returns - await task</returns>  
        private async Task SignInUser(string username, bool isPersistent)
        {
            // Initialization.  
            var claims = new List<Claim>();

            try
            {
                // Setting  
                claims.Add(new Claim(ClaimTypes.Name, username));

                //var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //var principal = new ClaimsPrincipal(identity);

                // await HttpContext.SignInAsync(principal,);

                var claimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdenties);
                var authenticationManager = Request.HttpContext;

                // Sign In.  
                await authenticationManager.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    claimPrincipal,
                    new AuthenticationProperties()
                    {
                        IsPersistent = isPersistent
                    });
            }
            catch (Exception)
            {
                // Info  
                throw;
            }
        }
    }
}
