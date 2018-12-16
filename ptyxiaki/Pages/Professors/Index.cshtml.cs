using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Professors
{
  public class IndexModel : PageModel
  {
    private readonly ptyxiaki.Data.DepartmentContext _context;

    public IndexModel(ptyxiaki.Data.DepartmentContext context)
    {
      _context = context;
    }

    public IList<Professor> Professor { get; set; }

    public async Task OnGetAsync()
    {
      Professor = await _context.professors.ToListAsync();
    }
  }
}
