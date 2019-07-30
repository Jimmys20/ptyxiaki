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
    public static int? getUserId(this ClaimsPrincipal claimsPrincipal)
    {
      var id = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

      if (int.TryParse(id, out int retVal))
      {
        return retVal;
      }

      return null;
    }
  }
}
