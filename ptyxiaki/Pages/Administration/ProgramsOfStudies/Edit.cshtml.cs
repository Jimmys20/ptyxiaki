using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Administration.ProgramsOfStudies
{
  public class EditModel : PageModel
  {
    private readonly DepartmentContext context;

    public EditModel(DepartmentContext context)
    {
      this.context = context;
    }

    [BindProperty]
    public ProgramOfStudies programOfStudies { get; set; }
    [BindProperty]
    [Required]
    [Display(Name = "Μαθήματα")]
    public IFormFile courses { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      programOfStudies = await context.programsOfStudies.FirstOrDefaultAsync(m => m.programOfStudiesId == id);

      if (programOfStudies == null)
      {
        return NotFound();
      }
      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      var dbProgramOfStudies = context.programsOfStudies
        .Include(p => p.courses)
        .FirstOrDefault(p => p.programOfStudiesId == programOfStudies.programOfStudiesId);

      dbProgramOfStudies.title = programOfStudies.title;

      using (var reader = new StreamReader(courses.OpenReadStream()))
      using (var csvr = new CsvReader(reader))
      {
        csvr.Configuration.Delimiter = "\t";
        csvr.Configuration.HeaderValidated = null;
        csvr.Configuration.MissingFieldFound = null;

        var records = csvr.GetRecords<Course>();

        dbProgramOfStudies.courses = records.ToList();
      }

      try
      {
        await context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!programOfStudiesExists(dbProgramOfStudies.programOfStudiesId))
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

    private bool programOfStudiesExists(int id)
    {
      return context.programsOfStudies.Any(e => e.programOfStudiesId == id);
    }
  }
}
