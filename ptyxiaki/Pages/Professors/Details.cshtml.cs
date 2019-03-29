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

namespace ptyxiaki.Pages.Professors
{
  public class DetailsModel : PageModel
  {
    private readonly DepartmentContext context;

    public DetailsModel(DepartmentContext context)
    {
      this.context = context;
    }

    public Professor professor { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (id == null && User.IsInRole(Globals.PROFESSOR_ROLE))
      {
        id = User.getUserId();
      }

      if (id == null)
      {
        return NotFound();
      }

      professor = await context.professors.FirstOrDefaultAsync(m => m.professorId == id);

      if (professor == null)
      {
        return NotFound();
      }
      return Page();
    }
  }
}
