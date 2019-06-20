using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Data;
using ptyxiaki.Models;
using ptyxiaki.Services;

namespace ptyxiaki.Pages.Theses
{
  public class TransferToAvailableModel : PageModel
  {
    private readonly DepartmentContext context;
    private readonly IAuthorizationService authorizationService;

    public TransferToAvailableModel(DepartmentContext context, IAuthorizationService authorizationService)
    {
      this.context = context;
      this.authorizationService = authorizationService;
    }

    [BindProperty]
    public Thesis thesis { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      thesis = await context.theses
        .Include(t => t.semester)
        .FirstOrDefaultAsync(t => t.thesisId == id);

      if (thesis == null)
      {
        return NotFound();
      }

      var authorizationResult = await authorizationService.AuthorizeAsync(User, thesis, Operations.TransferToAvailable);

      if (!authorizationResult.Succeeded)
      {
        if (User.Identity.IsAuthenticated)
        {
          return Forbid();
        }

        return Challenge();
      }

      return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      thesis = await context.theses.FindAsync(id);

      if (thesis == null)
      {
        return NotFound();
      }

      var authorizationResult = await authorizationService.AuthorizeAsync(User, thesis, Operations.TransferToAvailable);

      if (!authorizationResult.Succeeded)
      {
        if (User.Identity.IsAuthenticated)
        {
          return Forbid();
        }

        return Challenge();
      }

      thesis.status = Status.Available;

      try
      {
        await context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!thesisExists(thesis.thesisId))
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

    private bool thesisExists(int id)
    {
      return context.theses.Any(t => t.thesisId == id);
    }
  }
}