using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Common;
using ptyxiaki.Data;
using ptyxiaki.Extensions;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Students
{
  [Authorize(Policy = Globals.USER_POLICY)]
  public class DetailsModel : PageModel
  {
    private readonly DepartmentContext context;

    public DetailsModel(DepartmentContext context)
    {
      this.context = context;
    }

    public Student student { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (User.IsInRole(Globals.STUDENT_ROLE))
      {
        if (id != null)
        {
          return Forbid();
        }

        id = User.getUserId();
      }

      if (id == null)
      {
        return NotFound();
      }

      student = await context.students
        .Include(s => s.assignments).ThenInclude(a => a.thesis)
        .FirstOrDefaultAsync(s => s.studentId == id);

      if (student == null)
      {
        return NotFound();
      }

      student.grades = await context.grades
        .Where(g => g.spec_aem == student.registrationNumber)
        .ToListAsync();

      return Page();
    }
  }
}
