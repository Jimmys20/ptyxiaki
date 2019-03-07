using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ptyxiaki.Common;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Categories
{
  [Authorize(Policy = Globals.ADMINISTRATOR_POLICY)]
  public class CreateModel : PageModel
  {
    private readonly DepartmentContext context;

    public CreateModel(DepartmentContext context)
    {
      this.context = context;
    }

    public IActionResult OnGet()
    {
      return Page();
    }

    [BindProperty]
    public Category category { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      context.categories.Add(category);
      await context.SaveChangesAsync();

      return RedirectToPage("./Index");
    }
  }
}