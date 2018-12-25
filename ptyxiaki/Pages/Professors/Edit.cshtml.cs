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
  [Authorize(Policy = "Professor")]
  public class EditModel : PageModel
  {
    private readonly DepartmentContext _context;
    private readonly IMapper _mapper;

    public EditModel(DepartmentContext context, IMapper mapper)
    {
      _context = context;
      _mapper = mapper;
    }

    public Professor Professor { get; set; }
    [BindProperty]
    public ProfessorVm ProfessorVm { get; set; }

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

      ProfessorVm = _mapper.Map<ProfessorVm>(Professor);

      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      var professorId = User.GetUserId();
      var professor = await _context.FindAsync<Professor>(professorId);
      _mapper.Map(ProfessorVm, professor);

      _context.Attach(professor).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ProfessorExists(professorId))
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

    private bool ProfessorExists(int? id)
    {
      return _context.professors.Any(e => e.professorId == id);
    }
  }
}
