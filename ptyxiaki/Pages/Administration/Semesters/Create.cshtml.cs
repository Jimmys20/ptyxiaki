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
    public Semester semester { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      var theses = await context.theses.Where(t => t.status < Status.Canceled).ToListAsync();
      foreach (var thesis in theses)
      {
        thesis.status = Status.Canceled;
        thesis.cancelComment = "Λήξη εξαμήνου";
      }

      semester.createdAt = DateTime.Now;
      context.semesters.Add(semester);
      await context.SaveChangesAsync();

      return RedirectToPage("./Index");
    }
  }
}