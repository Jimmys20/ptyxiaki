using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Common;
using ptyxiaki.Data;
using ptyxiaki.Models;
using ptyxiaki.Services;

namespace ptyxiaki.Pages.Administration.Semesters
{
  public class CreateModel : PageModel
  {
    private readonly DepartmentContext context;
    private readonly IEmailService emailService;

    public CreateModel(DepartmentContext context, IEmailService emailService)
    {
      this.context = context;
      this.emailService = emailService;
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

      var availableTheses = await context.theses
        .Include(t => t.professor)
        .Where(t => t.status == Status.Available)
        .ToListAsync();

      foreach (var thesis in availableTheses)
      {
        thesis.status = Status.Unavailable;
      }

      var activeTheses = await context.theses
        .Include(t => t.professor)
        .Include(t => t.assignments).ThenInclude(a => a.student)
        .Where(t => t.status == Status.Active)
        .ToListAsync();

      var canceledTheses = new List<Thesis>();

      foreach (var thesis in activeTheses)
      {
        if (thesis.assignedAt.HasValue && thesis.assignedAt.Value.AddMonths(Globals.MAX_PREPARATION_TIME) < DateTime.Now)
        {
          thesis.status = Status.Canceled;
          thesis.cancelReason = $"Αυτόματη ακύρωση λόγω παρέλευσης της μέγιστης διάρκειας εκπόνησης ({Globals.MAX_PREPARATION_TIME} μήνες).";
          canceledTheses.Add(thesis);
        }
      }

      semester.createdAt = DateTime.Now;
      context.semesters.Add(semester);
      await context.SaveChangesAsync();

      foreach (var thesis in availableTheses)
      {
        var address = new EmailAddress(thesis.professor.fullName, thesis.professor.email);
        var subject = "ptyxiaki - μεταφορά στις μη διαθέσιμες";
        var text = $"Η διπλωματική εργασία «{thesis.title}» μεταφέρθηκε αυτόματα στις μη διαθέσιμες λόγω έναρξης νέου εξαμήνου.";
        emailService.sendEmail(address, subject, text);
      }

      foreach (var thesis in canceledTheses)
      {
        var addresses = thesis.assignments.Select(a => new EmailAddress(a.student.fullName, a.student.email)).ToList();
        addresses.Add(new EmailAddress(thesis.professor.fullName, thesis.professor.email));

        var subject = "ptyxiaki - ακύρωση";
        var text = $"Η διπλωματική εργασία «{thesis.title}» ακυρώθηκε αυτόματα λόγω παρέλευσης της μέγιστης διάρκειας εκπόνησης ({Globals.MAX_PREPARATION_TIME} μήνες).";
        emailService.sendEmail(addresses, subject, text);
      }

      return RedirectToPage("./Index");
    }
  }
}