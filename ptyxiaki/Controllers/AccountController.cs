using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Common;
using ptyxiaki.Data;
using ptyxiaki.Extensions;
using ptyxiaki.Models;

namespace ptyxiaki.Controllers
{
  public class AccountController : Controller
  {
    private readonly DepartmentContext context;

    public AccountController(DepartmentContext context)
    {
      this.context = context;
    }

    [HttpGet]
    public IActionResult Login(string returnUrl = "/")
    {
      var properties = new AuthenticationProperties()
      {
        RedirectUri = Url.Action("HandleExternalLogin", "Account", new { returnUrl })
      };

      return Challenge(properties, Globals.O_AUTH_SCHEME);
    }

    public async Task<IActionResult> HandleExternalLogin(string returnUrl = "/", string remoteError = null)
    {
      var result = await HttpContext.AuthenticateAsync(Globals.EXTERNAL_COOKIE_SCHEME);

      if (!result.Succeeded)
      {
        return Forbid();
      }

      var claimsPrincipal = result.Principal;
      var claims = new List<Claim>();

      if (claimsPrincipal.IsInRole(Globals.PROFESSOR_ROLE) || claimsPrincipal.FindFirstValue(Claims.REGISTRATION_NUMBER) == "134007")
      {
        var professor = await context.professors.FirstOrDefaultAsync(p => p.oAuthId == claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));

        if (professor == null)
        {
          professor = new Professor()
          {
            oAuthId = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier),
            firstName = claimsPrincipal.FindFirstValue(ClaimTypes.GivenName),
            lastName = claimsPrincipal.FindFirstValue(ClaimTypes.Surname),
            email = claimsPrincipal.FindFirstValue(ClaimTypes.Email),
            phone = claimsPrincipal.FindFirstValue(Claims.PHONE),
            website = claimsPrincipal.FindFirstValue(ClaimTypes.Webpage)
          };

          context.professors.Add(professor);
          await context.SaveChangesAsync();
        }

        claims.Add(new Claim(ClaimTypes.NameIdentifier, professor.professorId.ToString()));
        claims.Add(new Claim(ClaimTypes.Name, professor.fullName));
        claims.Add(new Claim(ClaimTypes.Role, Globals.PROFESSOR_ROLE));
        if (professor.isAdmin)
          claims.Add(new Claim(ClaimTypes.Role, Globals.ADMINISTRATOR_ROLE));
      }

      if (claimsPrincipal.IsInRole(Globals.STUDENT_ROLE))
      {
        var student = await context.students.FirstOrDefaultAsync(s => s.registrationNumber == claimsPrincipal.FindFirstValue(Claims.REGISTRATION_NUMBER));

        if (student != null)
        {
          claims.Add(new Claim(ClaimTypes.NameIdentifier, student.studentId.ToString()));
          claims.Add(new Claim(ClaimTypes.Name, student.fullName));
          claims.Add(new Claim(ClaimTypes.Role, Globals.STUDENT_ROLE));
        }
      }

      var claimsIdentity = new ClaimsIdentity(claims, Globals.APP_COOKIE_SCHEME);

      await HttpContext.SignInAsync(Globals.APP_COOKIE_SCHEME, new ClaimsPrincipal(claimsIdentity));
      await HttpContext.SignOutAsync(Globals.EXTERNAL_COOKIE_SCHEME);

      return LocalRedirect(returnUrl);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync(Globals.APP_COOKIE_SCHEME);

      return RedirectToPage("/Index");
    }
  }
}
