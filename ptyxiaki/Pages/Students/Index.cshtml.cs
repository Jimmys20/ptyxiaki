using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Students
{
  [Authorize(Policy = "Professor")]
  public class IndexModel : PageModel
  {
    private readonly DepartmentContext _context;

    public IndexModel(DepartmentContext context)
    {
      _context = context;
    }

    public IList<Student> Student { get; set; } = new List<Student>();
    [BindProperty(SupportsGet = true)]
    public string SearchString { get; set; }

    public async Task OnGetAsync()
    {
      if (!string.IsNullOrEmpty(SearchString))
      {
        Student = await _context.students
          .Where(s => s.lastName.Contains(SearchString) ||
                      s.registrationNumber.Contains(SearchString) ||
                      s.firstName.Contains(SearchString))
          .ToListAsync();
      }
    }
  }
}
