using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Common;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Categories
{
  [Authorize(Policy = Globals.ADMINISTRATOR_POLICY)]
  public class EditModel : PageModel
  {
    private readonly DepartmentContext context;

    public EditModel(DepartmentContext context)
    {
      this.context = context;
    }

    [BindProperty]
    public Category category { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      category = await context.categories.FirstOrDefaultAsync(m => m.categoryId == id);

      if (category == null)
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

      context.Attach(category).State = EntityState.Modified;

      try
      {
        await context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!CategoryExists(category.categoryId))
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

    private bool CategoryExists(int id)
    {
      return context.categories.Any(e => e.categoryId == id);
    }
  }
}
