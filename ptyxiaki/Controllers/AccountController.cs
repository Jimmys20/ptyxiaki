using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ptyxiaki.Controllers
{
  [Route("[controller]/[action]")]
  public class AccountController : Controller
  {
    [HttpGet]
    public IActionResult Login(string returnUrl = "/")
    {
      return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties() { RedirectUri = "/Index" });

      return RedirectToPage("/Index");
    }
  }
}
