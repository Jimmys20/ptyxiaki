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
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Categories
{
  [Authorize(Policy = Globals.ADMINISTRATOR_POLICY)]
  public class DeleteModel : PageModel
  {
    private readonly DepartmentContext context;

    public DeleteModel(DepartmentContext context)
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

    public async Task<IActionResult> OnPostAsync(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      category = await context.categories.FindAsync(id);

      if (category != null)
      {
        context.categories.Remove(category);
        await context.SaveChangesAsync();
      }

      return RedirectToPage("./Index");
    }
  }
}
