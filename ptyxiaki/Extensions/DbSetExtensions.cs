using Microsoft.EntityFrameworkCore;
using ptyxiaki.Common;
using ptyxiaki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Extensions
{
  public static class DbSetExtensions
  {
    public static async Task<int?> getCurrentSemesterIdAsync(this DbSet<Semester> semesters)
    {
      var semester = await semesters.OrderBy(s => s.createdAt).FirstOrDefaultAsync();
      return semester?.semesterId;
    }

    public static bool isPostPeriod(this DbSet<Date> dates)
    {
      var date = dates.FirstOrDefault();
      var now = DateTime.Now;

      if (date != null &&
          date.postStart.HasValue && now >= date.postStart.Value &&
          date.postEnd.HasValue && now <= date.postEnd.Value)
        return true;

      return false;
    }

    public static bool isDeclarationPeriod(this DbSet<Date> dates)
    {
      var date = dates.FirstOrDefault();
      var now = DateTime.Now;

      if (date != null &&
          date.declarationStart.HasValue && now >= date.declarationStart.Value &&
          date.declarationEnd.HasValue && now <= date.declarationEnd.Value)
        return true;

      return false;
    }

    public static IQueryable<Student> getStudentsWhoMeetRequirements(this DbSet<Student> students)
    {
      return students.Where(s => s.semester >= Globals.STUDENT_SEMESTER_REQUIREMENT &&
                                 s.credits >= Globals.STUDENT_CREDITS_REQUIREMENT &&
                                 !s.assignments.Any(a => a.thesis.status != Status.Canceled));
    }
  }
}
