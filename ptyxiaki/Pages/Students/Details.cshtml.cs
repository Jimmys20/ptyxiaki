using System;
using System.Collections.Generic;
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
  [Authorize]
  public class DetailsModel : PageModel
  {
    private readonly DepartmentContext _context;

    public DetailsModel(DepartmentContext context)
    {
      _context = context;
    }

    public Student Student { get; set; }
    [BindProperty(SupportsGet = true)]
    public string SearchString { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (User.IsInRole(Globals.StudentRole))
      {
        id = User.GetUserId();
      }

      if (id == null)
      {
        return NotFound();
      }

      Student = await _context.students.FirstOrDefaultAsync(m => m.studentId == id);

      if (Student == null)
      {
        return NotFound();
      }
      return Page();
    }
  }
}
