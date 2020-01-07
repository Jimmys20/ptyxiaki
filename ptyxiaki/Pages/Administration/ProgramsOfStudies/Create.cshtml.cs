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
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Administration.ProgramsOfStudies
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
    public ProgramOfStudies programOfStudies { get; set; }
    [BindProperty]
    [Required]
    [Display(Name = "Μαθήματα")]
    public IFormFile courses { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      using (var reader = new StreamReader(courses.OpenReadStream()))
      using (var csvr = new CsvReader(reader))
      {
        csvr.Configuration.Delimiter = "\t";
        csvr.Configuration.HeaderValidated = null;
        csvr.Configuration.MissingFieldFound = null;

        var records = csvr.GetRecords<Course>();

        programOfStudies.courses = records.ToList();
      }

      context.programsOfStudies.Add(programOfStudies);
      await context.SaveChangesAsync();

      return RedirectToPage("./Index");
    }
  }
}
