using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Common;
using ptyxiaki.Data;
using ptyxiaki.Extensions;
using ptyxiaki.Models;
using ptyxiaki.Services;

namespace ptyxiaki.Pages.Theses
{
  public class CreateModel : PageModel
  {
    private readonly DepartmentContext context;
    private readonly IMapper mapper;
    private readonly IAuthorizationService authorizationService;
    private readonly IEmailService emailService;

    public CreateModel(DepartmentContext context,
                       IMapper mapper,
                       IAuthorizationService authorizationService,
                       IEmailService emailService)
    {
      this.context = context;
      this.mapper = mapper;
      this.authorizationService = authorizationService;
      this.emailService = emailService;
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      Thesis thesis = null;
      var operation = Operations.Create;

      if (id != null)
      {
        thesis = await context.theses
          .Include(t => t.categorizations)
          .Include(t => t.requirements)
          .FirstOrDefaultAsync(t => t.thesisId == id);

        if (thesis != null)
        {
          thesisVm = mapper.Map<ThesisVmProfessor>(thesis);
          operation = Operations.Copy;
        }
      }

      var authorizationResult = await authorizationService.AuthorizeAsync(User, thesis ?? new Thesis(), operation);

      if (!authorizationResult.Succeeded)
      {
        if (User.Identity.IsAuthenticated)
        {
          return Forbid();
        }

        return Challenge();
      }

      populateSelectLists();
      return Page();
    }

    [BindProperty]
    public ThesisVmProfessor thesisVm { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
      var authorizationResult = await authorizationService.AuthorizeAsync(User, new Thesis(), Operations.Create);

      if (!authorizationResult.Succeeded)
      {
        if (User.Identity.IsAuthenticated)
        {
          return Forbid();
        }

        return Challenge();
      }

      if (!ModelState.IsValid)
      {
        populateSelectLists();
        return Page();
      }

      Thesis thesis = mapper.Map<Thesis>(thesisVm);

      var professorId = User.getUserId();

      if (professorId.HasValue)
      {
        thesis.professorId = professorId.Value;
      }
      else
      {
        ModelState.AddModelError(string.Empty, "Δεν βρέθηκε το id του χρήστη. Επικοινωνήστε με τον διαχειριστή του συστήματος.");
        populateSelectLists();
        return Page();
      }

      var semester = await context.semesters.getCurrentSemesterAsync();

      if (semester != null)
      {
        thesis.semesterId = semester.semesterId;
      }
      else
      {
        ModelState.AddModelError(string.Empty, "Δεν έχει οριστεί τρέχον εξάμηνο. Επικοινωνήστε με τον διαχειριστή του συστήματος.");
        populateSelectLists();
        return Page();
      }

      thesis.createdAt = DateTime.Now;
      thesis.status = Status.Available;

      if (thesis.assignments.Any())
      {
        thesis.assignedAt = DateTime.Now;
        thesis.status = Status.Active;
      }

      context.theses.Add(thesis);
      await context.SaveChangesAsync();

      if (thesis.assignments.Any())
      {
        var assignments = await context.assignments
          .Include(a => a.student)
          .Where(a => a.thesisId == thesis.thesisId)
          .ToListAsync();

        var addresses = assignments.Select(a => new EmailAddress(a.student.fullName, a.student.email));
        var subject = "ptyxiaki - ανάθεση";
        var text = $"Σας ανατέθηκε η διπλωματική εργασία «{thesis.title}».";
        emailService.sendEmail(addresses, subject, text);
      }

      return RedirectToPage("./Index");
    }

    private void populateSelectLists()
    {
      var students = context.students
        .getStudentsWhoMeetRequirements()
        .OrderBy(s => s.lastName).ThenBy(s => s.firstName);

      var currentProgramOfStudiesId = context.programsOfStudies.getCurrentProgramOfStudiesId();

      var courses = context.courses
        .Where(c => c.programOfStudiesId == currentProgramOfStudiesId)
        .OrderBy(c => c.code);

      ViewData["students"] = new SelectList(students, "studentId", "registrationNumberAndFullName");
      ViewData["courses"] = new SelectList(courses, "courseId", "title");
      ViewData["categories"] = new SelectList(context.categories, "categoryId", "title");
    }
  }
}