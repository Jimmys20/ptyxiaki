using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Data;
using ptyxiaki.Extensions;
using ptyxiaki.Models;
using ptyxiaki.Services;

namespace ptyxiaki.Pages.Theses
{
  public class CancelModel : PageModel
  {
    private readonly DepartmentContext context;
    private readonly IAuthorizationService authorizationService;
    private readonly IEmailService emailService;

    public CancelModel(DepartmentContext context,
                       IAuthorizationService authorizationService,
                       IEmailService emailService)
    {
      this.context = context;
      this.authorizationService = authorizationService;
      this.emailService = emailService;
    }

    [BindProperty]
    public Thesis thesis { get; set; }
    [BindProperty]
    [Required]
    [Display(Name = "Λόγος ακύρωσης")]
    public string cancelReason { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      thesis = await context.theses
        .Include(t => t.semester)
        .Include(t => t.assignments).ThenInclude(a => a.student)
        .FirstOrDefaultAsync(t => t.thesisId == id);

      if (thesis == null)
      {
        return NotFound();
      }

      var authorizationResult = await authorizationService.AuthorizeAsync(User, thesis, Operations.Cancel);

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

      thesis = await context.theses
        .Include(t => t.assignments).ThenInclude(a => a.student)
        .FirstOrDefaultAsync(t => t.thesisId == id);

      if (thesis == null)
      {
        return NotFound();
      }

      var authorizationResult = await authorizationService.AuthorizeAsync(User, thesis, Operations.Cancel);

      if (!authorizationResult.Succeeded)
      {
        if (User.Identity.IsAuthenticated)
        {
          return Forbid();
        }

        return Challenge();
      }

      thesis.status = Status.Canceled;
      thesis.canceledAt = DateTime.Now;
      thesis.cancelReason = cancelReason;

      try
      {
        await context.SaveChangesAsync();

        var addresses = thesis.assignments.Select(a => new EmailAddress(a.student.fullName, a.student.email));
        var subject = "ptyxiaki - ακύρωση";
        var text = $"Η διπλωματική εργασία «{thesis.title}» ακυρώθηκε από τον επιβλέποντα καθηγητή.";
        emailService.sendEmail(addresses, subject, text);
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

      return RedirectToPage("./Index", new { status = Status.Canceled });
    }

    private bool thesisExists(int id)
    {
      return context.theses.Any(t => t.thesisId == id);
    }
  }
}