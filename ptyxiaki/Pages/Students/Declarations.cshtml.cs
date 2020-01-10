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

namespace ptyxiaki.Pages.Students
{
  [Authorize(Policy = Globals.STUDENT_POLICY)]
  public class DeclarationsModel : PageModel
  {
    private readonly DepartmentContext context;
    private readonly IAuthorizationService authorizationService;

    public DeclarationsModel(DepartmentContext context, IAuthorizationService authorizationService)
    {
      this.context = context;
      this.authorizationService = authorizationService;
    }

    public List<Declaration> declarations { get; set; }
    [BindProperty(SupportsGet = true)]
    public BootstrapAlert alert { get; set; }

    public async Task OnGetAsync()
    {
      var currentSemester = await context.semesters.getCurrentSemesterAsync();

      if (currentSemester != null)
      {
        declarations = await context.declarations
          .Include(d => d.thesis).ThenInclude(t => t.professor)
          .Where(d => d.studentId == User.getUserId() &&
                      d.semesterId == currentSemester.semesterId)
          .ToListAsync();
      }
    }

    public async Task<IActionResult> OnPostAddAsync(int id)
    {
      var thesis = await context.theses.FirstOrDefaultAsync(t => t.thesisId == id);

      if (thesis == null)
      {
        return NotFound();
      }

      var authorizationResult = await authorizationService.AuthorizeAsync(User, thesis, Operations.Declare);

      if (!authorizationResult.Succeeded)
      {
        if (User.Identity.IsAuthenticated)
        {
          return Forbid();
        }

        return Challenge();
      }

      var currentSemester = await context.semesters.getCurrentSemesterAsync();

      var declarations = await context.declarations
        .Where(d => d.studentId == User.getUserId() &&
                    d.semesterId == currentSemester.semesterId)
        .ToListAsync();

      if (declarations.Count() >= Globals.MAX_DECLARATIONS)
      {
        return RedirectToPage(new BootstrapAlert("alert-danger",
          $"Έχετε ξεπεράσει τον μέγιστο αριθμό διπλωματικών εργασιών που μπορείτε να δηλώσετε ({Globals.MAX_DECLARATIONS}). Για να προσθέσετε μία νέα, πρέπει πρώτα να αφαιρέσετε κάποια άλλη."));
      }
      else if (declarations.Any(d => d.thesisId == id))
      {
        return RedirectToPage(new BootstrapAlert("alert-info",
          "Η διπλωματική εργασία έχει ήδη προστεθεί στην δήλωση σας."));
      }

      context.declarations.Add(new Declaration
      {
        thesisId = thesis.thesisId,
        studentId = User.getUserId().Value,
        semesterId = currentSemester.semesterId
      });

      await context.SaveChangesAsync();

      return RedirectToPage(new BootstrapAlert("alert-success",
        "Η διπλωματική εργασία προστέθηκε στην δήλωση σας."));
    }

    public async Task<IActionResult> OnPostRemoveAsync(int id)
    {
      if (!context.dates.isDeclarationPeriod())
      {
        return Forbid();
      }

      var currentSemester = await context.semesters.getCurrentSemesterAsync();

      var declaration = await context.declarations
        .FirstOrDefaultAsync(d => d.declarationId == id &&
                                  d.studentId == User.getUserId() &&
                                  d.semesterId == currentSemester.semesterId);

      context.declarations.Remove(declaration);
      await context.SaveChangesAsync();

      return RedirectToPage(new BootstrapAlert("alert-success",
        "Η διπλωματική εργασία αφαιρέθηκε από την δήλωση σας."));
    }
  }
}
