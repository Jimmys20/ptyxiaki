using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Professors
{
  public class DetailsModel : PageModel
  {
    private readonly ptyxiaki.Data.DepartmentContext _context;

    public DetailsModel(ptyxiaki.Data.DepartmentContext context)
    {
      _context = context;
    }

    public Professor Professor { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
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
