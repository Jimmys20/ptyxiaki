using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Npgsql.Bulk;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Administrator.Data
{
  public class ImportModel : PageModel
  {
    private readonly DepartmentContext context;
    private readonly IMapper mapper;

    public ImportModel(DepartmentContext context, IMapper mapper)
    {
      this.context = context;
      this.mapper = mapper;
    }

    [BindProperty]
    public DataImportFiles files { get; set; }
    [BindProperty(SupportsGet = true)]
    public BootstrapAlert alert { get; set; }

    public async Task OnGetAsync()
    {
      await Task.CompletedTask;
    }

    public async Task<IActionResult> OnPostAsync()
    {
      try
      {
        await importStudentsAsync();
        await importGradesAsync();

        return RedirectToPage(new BootstrapAlert("alert-success", "Τα δεδομένα αποθηκεύτηκαν με επιτυχία."));
      }
      catch (Exception ex)
      {
        return RedirectToPage(new BootstrapAlert("alert-danger", ex.Message));
      }
    }

    public async Task importStudentsAsync()
    {
      IFormFile students = files.students;

      if (students != null)
      {
        using (var reader = new StreamReader(students.OpenReadStream()))
        using (var csvr = new CsvReader(reader))
        {
          csvr.Configuration.Delimiter = "\t";
          csvr.Configuration.HeaderValidated = null;
          csvr.Configuration.MissingFieldFound = null;

          var recordsVm = csvr.GetRecords<StudentVm>();
          var records = mapper.Map<List<Student>>(recordsVm);

          var uploader = new NpgsqlBulkUploader(context);
          await uploader.InsertAsync(records,
            InsertConflictAction.UpdateProperty<Student>(s => s.registrationNumber,
              s => s.firstName,
              s => s.lastName,
              s => s.fatherName,
              s => s.semester,
              s => s.average,
              s => s.credits,
              s => s.progressFactor));
        }
      }
    }

    //public async Task importCoursesAsync()
    //{
    //  IFormFile courses = files.courses;

    //  if (courses != null)
    //  {
    //    using (var reader = new StreamReader(courses.OpenReadStream()))
    //    using (var csvr = new CsvReader(reader))
    //    {
    //      csvr.Configuration.Delimiter = "\t";
    //      csvr.Configuration.HeaderValidated = null;
    //      csvr.Configuration.MissingFieldFound = null;

    //      var records = csvr.GetRecords<Course>();
    //    }
    //  }
    //}

    public async Task importGradesAsync()
    {
      IFormFile grades = files.grades;

      if (grades != null)
      {
        using (var reader = new StreamReader(grades.OpenReadStream()))
        using (var csvr = new CsvReader(reader))
        {
          csvr.Configuration.Delimiter = "\t";
          csvr.Configuration.HeaderValidated = null;
          csvr.Configuration.MissingFieldFound = null;

          var records = csvr.GetRecords<Grade>();

          await context.Database.ExecuteSqlRawAsync("TRUNCATE grades RESTART IDENTITY");

          var uploader = new NpgsqlBulkUploader(context);
          await uploader.InsertAsync(records);
        }
      }
    }
  }
}