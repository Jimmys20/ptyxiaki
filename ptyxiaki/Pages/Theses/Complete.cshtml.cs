using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
  public class CompleteModel : PageModel
  {
    private readonly DepartmentContext context;
    private readonly IAuthorizationService authorizationService;
    private readonly IEmailService emailService;
    private readonly IWebHostEnvironment environment;

    public CompleteModel(DepartmentContext context,
                         IAuthorizationService authorizationService,
                         IEmailService emailService,
                         IWebHostEnvironment environment)
    {
      this.context = context;
      this.authorizationService = authorizationService;
      this.emailService = emailService;
      this.environment = environment;
    }

    [BindProperty]
    public Thesis thesis { get; set; }
    [BindProperty]
    [Required]
    [Display(Name = "Αρχείο")]
    public IFormFile upload { get; set; }

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

      var authorizationResult = await authorizationService.AuthorizeAsync(User, thesis, Operations.Complete);

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

      var authorizationResult = await authorizationService.AuthorizeAsync(User, thesis, Operations.Complete);

      if (!authorizationResult.Succeeded)
      {
        if (User.Identity.IsAuthenticated)
        {
          return Forbid();
        }

        return Challenge();
      }

      thesis.status = Status.Completed;
      thesis.completedAt = DateTime.Now;

      var extension = Path.GetExtension(upload.FileName);

      if (extension.ToUpper() != ".PDF")
      {
        ModelState.AddModelError(nameof(upload), "Το αρχείο πρέπει να είναι σε μορφή PDF.");
        return Page();
      }

      var fileName = $"{thesis.completedAt.Value.ToString("yyyy-MM-dd")}";
      foreach (var student in thesis.assignments.Select(a => a.student))
      {
        fileName += $"_{student.registrationNumber}";
      }
      fileName += extension;

      var file = Path.Combine(environment.ContentRootPath, "theses", fileName);
      using (var fileStream = new FileStream(file, FileMode.Create))
      {
        await upload.CopyToAsync(fileStream);
      }

      thesis.filePath = file;

      try
      {
        await context.SaveChangesAsync();

        var addresses = thesis.assignments.Select(a => new EmailAddress(a.student.fullName, a.student.email));
        var subject = "ptyxiaki - ολοκλήρωση";
        var text = $"Η διπλωματική εργασία «{thesis.title}» ολοκληρώθηκε από τον επιβλέποντα καθηγητή.";
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

      return RedirectToPage("./Index", new { status = Status.Completed });
    }

    private bool thesisExists(int id)
    {
      return context.theses.Any(t => t.thesisId == id);
    }
  }
}