using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Common;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Students
{
  [Authorize(Policy = Globals.ProfessorPolicy)]
  public class IndexModel : PageModel
  {
    private readonly DepartmentContext _context;

    public IndexModel(DepartmentContext context)
    {
      _context = context;
    }

    public List<Student> Student { get; set; }

    public async Task OnGetAsync()
    {
      await Task.CompletedTask;
    }
  }
}
