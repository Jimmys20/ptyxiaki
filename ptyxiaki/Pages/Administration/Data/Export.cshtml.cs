using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Administration.Data
{
  public class ExportModel : PageModel
  {
    private readonly DepartmentContext context;
    private readonly IMapper mapper;

    public ExportModel(DepartmentContext context, IMapper mapper)
    {
      this.context = context;
      this.mapper = mapper;
    }

    [BindProperty(SupportsGet = true)]
    [Display(Name = "Εξάμηνο")]
    public int? semesterId { get; set; }

    public void OnGet()
    {
      ViewData["semesters"] = new SelectList(context.semesters, "semesterId", "title");
    }

    public async Task<FileResult> OnGetDownloadAsync()
    {
      var queryable = context.theses.AsQueryable();

      if (semesterId != null)
      {
        queryable = queryable.Where(t => t.semesterId == semesterId);
      }

      var theses = await queryable
        .Include(t => t.semester)
        .Include(t => t.professor)
        .Include(t => t.assignments).ThenInclude(a => a.student)
        .OrderBy(t => t.status)
        .ToListAsync();

      var thesesExport = mapper.Map<List<ThesisExport>>(theses);

      var memoryStream = new MemoryStream();
      var writer = new StreamWriter(memoryStream);
      var csv = new CsvWriter(writer);
      csv.Configuration.Delimiter = "\t";

      csv.WriteRecords(thesesExport);
      writer.Flush();
      memoryStream.Position = 0;

      return File(memoryStream, "application/octet-stream", fileDownloadName: "theses.csv");
    }
  }
}
