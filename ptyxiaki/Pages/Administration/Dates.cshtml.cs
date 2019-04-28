using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Administration
{
  public class DatesModel : PageModel
  {
    private readonly DepartmentContext context;

    public DatesModel(DepartmentContext context)
    {
      this.context = context;
    }

    [BindProperty]
    public Date date { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
      date = await context.dates.FirstOrDefaultAsync();

      if (date == null)
      {
        date = new Date();
        context.dates.Add(date);
        await context.SaveChangesAsync();
      }
      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      context.Attach(date).State = EntityState.Modified;

      try
      {
        await context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!dateExists(date.dateId))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }

      return RedirectToPage("./Dates");
    }

    private bool dateExists(int id)
    {
      return context.dates.Any(e => e.dateId == id);
    }
  }
}
