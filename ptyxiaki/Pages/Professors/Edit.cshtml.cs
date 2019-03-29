using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Common;
using ptyxiaki.Data;
using ptyxiaki.Extensions;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Professors
{
  [Authorize(Policy = Globals.PROFESSOR_POLICY)]
  public class EditModel : PageModel
  {
    private readonly DepartmentContext context;
    private readonly IMapper mapper;

    public EditModel(DepartmentContext context, IMapper mapper)
    {
      this.context = context;
      this.mapper = mapper;
    }

    [BindProperty]
    public ProfessorVm professorVm { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
      var id = User.getUserId();

      if (id == null)
      {
        return NotFound();
      }

      var professor = await context.professors.FirstOrDefaultAsync(m => m.professorId == id);

      if (professor == null)
      {
        return NotFound();
      }

      professorVm = mapper.Map<ProfessorVm>(professor);

      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      var id = User.getUserId();
      var professor = await context.professors.FirstOrDefaultAsync(p => p.professorId == id);
      mapper.Map(professorVm, professor);

      try
      {
        await context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!professorExists(professor.professorId))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return RedirectToPage("./Details");
    }

    private bool professorExists(int? id)
    {
      return context.professors.Any(e => e.professorId == id);
    }
  }
}
