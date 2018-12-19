using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Students
{
  public class IndexModel : PageModel
  {
    private readonly DepartmentContext _context;

    public IndexModel(DepartmentContext context)
    {
      _context = context;
    }

    public IList<Student> Student { get; set; }

    public async Task OnGetAsync()
    {
      Student = await _context.students.ToListAsync();
    }
  }
}
