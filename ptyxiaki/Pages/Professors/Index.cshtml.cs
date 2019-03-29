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
    private readonly DepartmentContext context;

    public IndexModel(DepartmentContext context)
    {
      this.context = context;
    }

    public List<Professor> professors { get; set; }

    public async Task OnGetAsync()
    {
      professors = await context.professors.ToListAsync();
    }
  }
}
