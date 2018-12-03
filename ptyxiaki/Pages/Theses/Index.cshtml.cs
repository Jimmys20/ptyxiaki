using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Theses
{
    public class IndexModel : PageModel
    {
        private readonly ptyxiaki.Data.DepartmentContext _context;

        public IndexModel(ptyxiaki.Data.DepartmentContext context)
        {
            _context = context;
        }

        public IList<Thesis> Thesis { get;set; }

        public async Task OnGetAsync()
        {
            Thesis = await _context.theses
                .Include(t => t.professor).ToListAsync();
        }
    }
}
