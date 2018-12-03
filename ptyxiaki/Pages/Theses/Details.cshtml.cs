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
    public class DetailsModel : PageModel
    {
        private readonly ptyxiaki.Data.DepartmentContext _context;

        public DetailsModel(ptyxiaki.Data.DepartmentContext context)
        {
            _context = context;
        }

        public Thesis Thesis { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Thesis = await _context.theses
                .Include(t => t.professor).FirstOrDefaultAsync(m => m.thesisId == id);

            if (Thesis == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
