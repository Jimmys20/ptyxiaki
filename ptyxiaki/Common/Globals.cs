using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Common
{
  public static class Globals
  {
    public const string APP_COOKIE_SCHEME = "APP_COOKIE_SCHEME";
    public const string EXTERNAL_COOKIE_SCHEME = "EXTERNAL_COOKIE_SCHEME";
    public const string O_AUTH_SCHEME = "O_AUTH_SCHEME";

    public const string STUDENT_ROLE = "student";
    public const string PROFESSOR_ROLE = "staff";
    public const string ADMINISTRATOR_ROLE = "ADMINISTRATOR_ROLE";

    public const string USER_POLICY = "USER_POLICY";
    public const string STUDENT_POLICY = "STUDENT_POLICY";
    public const string PROFESSOR_POLICY = "PROFESSOR_POLICY";
    public const string ADMINISTRATOR_POLICY = "ADMINISTRATOR_POLICY";

    public const int STUDENT_SEMESTER_REQUIREMENT = 7; //10
    public const int STUDENT_CREDITS_REQUIREMENT = 160; //210
    public const int MAX_DECLARATIONS = 5;
    public const int MAX_PREPARATION_TIME = 24; //months
  }

  public static class Claims
  {
    public const string REGISTRATION_NUMBER = "REGISTRATION_NUMBER";
    public const string PHONE = "PHONE";
  }
}
