using ptyxiaki.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ptyxiaki.Extensions
{
  public static class ClaimsPrincipalExtensions
  {
    public static int? GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
      var id = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

      if (!int.TryParse(id, out int retVal))
      {
        return null;
      }

      return retVal;
    }

    public static bool HasRole(this ClaimsPrincipal claimsPrincipal, string role)
    {
      return claimsPrincipal.HasClaim(ClaimTypes.Role, role);
    }
  }
}
