using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Common
{
  public static class Globals
  {
    public const string AppCookieScheme = "AppCookie";
    public const string ExternalCookieScheme = "ExternalCookie";
    public const string OAuthScheme = "OAuth";

    public const string StudentRole = "student";
    public const string ProfessorRole = "staff";

    public const string UserPolicy = "User";
    public const string StudentPolicy = "Student";
    public const string ProfessorPolicy = "Professor";
    public const string AdministratorPolicy = "Administrator";
  }

  public static class Claims
  {
    public const string RegistrationNumber = "urn:oauth:registrationnumber";
    public const string Phone = "urn:oauth:phone";
  }
}
