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
using ptyxiaki.Extensions;
using ptyxiaki.Models;
using ptyxiaki.Services;

namespace ptyxiaki.Pages.Theses
{
  public class DetailsModel : PageModel
  {
    private readonly DepartmentContext context;
    private readonly IAuthorizationService authorizationService;

    public DetailsModel(DepartmentContext context, IAuthorizationService authorizationService)
    {
      this.context = context;
      this.authorizationService = authorizationService;
    }

    public Thesis thesis { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      thesis = await context.theses
        .Include(t => t.professor)
        .Include(t => t.semester)
        .Include(t => t.assignments).ThenInclude(a => a.student)
        .Include(t => t.categorizations).ThenInclude(c => c.category)
        .Include(t => t.requirements).ThenInclude(r => r.course)
        .Include(t => t.declarations).ThenInclude(d => d.student)
        .FirstOrDefaultAsync(t => t.thesisId == id);

      if (thesis == null)
      {
        return NotFound();
      }

      var authorizationResult = await authorizationService.AuthorizeAsync(User, thesis, Operations.Details);

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
  }
}
