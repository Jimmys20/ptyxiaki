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
  public class EditModel : PageModel
  {
    private readonly DepartmentContext context;
    private readonly IMapper mapper;
    private readonly IAuthorizationService authorizationService;

    public EditModel(DepartmentContext context, IMapper mapper, IAuthorizationService authorizationService)
    {
      this.context = context;
      this.mapper = mapper;
      this.authorizationService = authorizationService;
    }

    [BindProperty]
    public int thesisId { get; set; }
    [BindProperty]
    public ThesisVmProfessor thesisVmProfessor { get; set; }
    [BindProperty]
    public ThesisVmAdministrator thesisVmAdministrator { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var thesis = await context.theses
        .Include(t => t.requirements)
        .Include(t => t.categorizations)
        .Include(t => t.professor)
        .FirstOrDefaultAsync(m => m.thesisId == id);

      if (thesis == null)
      {
        return NotFound();
      }

      var authorizationResult = await authorizationService.AuthorizeAsync(User, thesis, Operations.Edit);

      if (!authorizationResult.Succeeded)
      {
        if (User.Identity.IsAuthenticated)
        {
          return Forbid();
        }

        return Challenge();
      }

      thesisId = thesis.thesisId;
      thesisVmProfessor = mapper.Map<ThesisVmProfessor>(thesis);
      thesisVmAdministrator = mapper.Map<ThesisVmAdministrator>(thesis);

      populateSelectLists();
      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        populateSelectLists();
        return Page();
      }

      var thesis = await context.theses
        .Include(t => t.categorizations)
        .Include(t => t.requirements)
        .Include(t => t.assignments)
        .FirstOrDefaultAsync(t => t.thesisId == thesisId);

      if (thesis == null)
      {
        return NotFound();
      }

      var authorizationResult = await authorizationService.AuthorizeAsync(User, thesis, Operations.Edit);

      if (!authorizationResult.Succeeded)
      {
        if (User.Identity.IsAuthenticated)
        {
          return Forbid();
        }

        return Challenge();
      }

      mapper.Map(thesisVmProfessor, thesis);

      if (User.IsInRole(Globals.ADMINISTRATOR_ROLE))
      {
        mapper.Map(thesisVmAdministrator, thesis);
      }

      if (thesis.assignments.Any() && !thesis.assignedAt.HasValue)
      {
        thesis.assignedAt = DateTime.Now;
        thesis.status = Status.Active;
      }

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
      return context.theses.Any(e => e.thesisId == id);
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

      if (User.IsInRole(Globals.ADMINISTRATOR_ROLE))
      {
        ViewData["professors"] = new SelectList(context.professors, "professorId", "fullName");
      }
    }
  }
}
