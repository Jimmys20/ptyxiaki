using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Data;
using ptyxiaki.Extensions;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Professors
{
  [Authorize(Policy = "Professor")]
  public class DetailsModel : PageModel
  {
    private readonly DepartmentContext _context;

    public DetailsModel(DepartmentContext context)
    {
      _context = context;
    }

    public Professor Professor { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
      var id = User.GetUserId();

      if (id == null)
      {
        return NotFound();
      }

      Professor = await _context.professors.FirstOrDefaultAsync(m => m.professorId == id);

      if (Professor == null)
      {
        return NotFound();
      }
      return Page();
    }
  }
}
