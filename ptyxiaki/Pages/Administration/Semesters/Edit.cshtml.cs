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

namespace ptyxiaki.Pages.Administration.Semesters
{
  public class EditModel : PageModel
  {
    private readonly DepartmentContext context;

    public EditModel(DepartmentContext context)
    {
      this.context = context;
    }

    [BindProperty]
    public Semester semester { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      semester = await context.semesters.FirstOrDefaultAsync(m => m.semesterId == id);

      if (semester == null)
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

      context.Attach(semester).State = EntityState.Modified;

      try
      {
        await context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!semesterExists(semester.semesterId))
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

    private bool semesterExists(int id)
    {
      return context.semesters.Any(e => e.semesterId == id);
    }
  }
}
