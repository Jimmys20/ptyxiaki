using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Professors
{
  public class EditModel : PageModel
  {
    private readonly ptyxiaki.Data.DepartmentContext _context;

    public EditModel(ptyxiaki.Data.DepartmentContext context)
    {
      _context = context;
    }

    [BindProperty]
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

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      _context.Attach(Professor).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!ProfessorExists(Professor.professorId))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return RedirectToPage("./Index");
    }

    private bool ProfessorExists(int id)
    {
      return _context.professors.Any(e => e.professorId == id);
    }
  }
}
