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
  [Route("[controller]/[action]")]
  public class AccountController : Controller
  {
    private DepartmentContext context;

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

      return Challenge(properties, Globals.OAuthScheme);
    }

    public async Task<IActionResult> HandleExternalLogin(string returnUrl = "/", string remoteError = null)
    {
      var result = await HttpContext.AuthenticateAsync(Globals.ExternalCookieScheme);

      if (!result.Succeeded)
      {
        return Forbid();
      }

      var claimsPrincipal = result.Principal;
      var claims = new List<Claim>();
      var identity = claimsPrincipal.Identity as ClaimsIdentity;
      identity.AddClaim(new Claim("", ""));

      if (claimsPrincipal.IsInRole(Globals.ProfessorRole))
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
            phone = claimsPrincipal.FindFirstValue(Claims.Phone)
          };

          context.professors.Add(professor);
          await context.SaveChangesAsync();
        }

        claims.Add(new Claim(ClaimTypes.NameIdentifier, professor.professorId.ToString()));
        claims.Add(new Claim(ClaimTypes.Name, professor.fullName));
        claims.Add(new Claim(ClaimTypes.Role, Globals.ProfessorRole));
      }
      else if (claimsPrincipal.IsInRole(Globals.StudentRole))
      {
        var student = await context.students.FirstOrDefaultAsync(s => s.registrationNumber == claimsPrincipal.FindFirstValue(Claims.RegistrationNumber));

        if (student != null)
        {
          claims.Add(new Claim(ClaimTypes.NameIdentifier, student.studentId.ToString()));
          claims.Add(new Claim(ClaimTypes.Name, student.fullName));
          claims.Add(new Claim(ClaimTypes.Role, Globals.StudentRole));
        }
      }

      var claimsIdentity = new ClaimsIdentity(claims, Globals.AppCookieScheme);

      await HttpContext.SignInAsync(Globals.AppCookieScheme, new ClaimsPrincipal(claimsIdentity));
      await HttpContext.SignOutAsync(Globals.ExternalCookieScheme);

      return LocalRedirect(returnUrl);
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync(Globals.AppCookieScheme);

      return RedirectToPage("/Index");
    }
  }
}
