using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Common
{
  public static class Globals
  {
    public static readonly string AppCookieScheme = "AppCookie";
    public static readonly string ExternalCookieScheme = "ExternalCookie";
    public static readonly string OAuthScheme = "OAuth";

    public static readonly string ProfessorRole = "staff";
    public static readonly string StudentRole = "student";

    public static readonly string ProfessorPolicy = "Professor";
    public static readonly string StudentPolicy = "Student";
  }

  public static class Claims
  {
    public static readonly string RegistrationNumber = "urn:oauth:registrationnumber";
    public static readonly string Phone = "urn:oauth:phone";
  }
}
