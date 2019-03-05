﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

namespace ptyxiaki.Pages.Students
{
  [Authorize(Policy = Globals.UserPolicy)]
  public class DetailsModel : PageModel
  {
    private readonly DepartmentContext _context;

    public DetailsModel(DepartmentContext context)
    {
      _context = context;
    }

    public Student Student { get; set; }
    [Display(Name = "Τρέχουσα πτυχιακή εργασία")]
    public Thesis ActiveThesis { get; set; }
    [Display(Name = "Ακυρωμένες πτυχιακές εργασίες")]
    public List<Thesis> CanceledTheses { get; set; }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
      if (User.IsInRole(Globals.StudentRole))
      {
        if (id != null)
        {
          return Forbid();
        }

        id = User.GetUserId();
      }

      if (id == null)
      {
        return NotFound();
      }

      Student = await _context.students
        .Include(s => s.assignments).ThenInclude(a => a.thesis)
        .FirstOrDefaultAsync(s => s.studentId == id);

      if (Student == null)
      {
        return NotFound();
      }

      var theses = Student.assignments.Select(a => a.thesis);
      ActiveThesis = theses.FirstOrDefault(t => t.status == Status.Active);
      CanceledTheses = theses.Where(t => t.status == Status.Canceled).ToList();

      return Page();
    }
  }
}
