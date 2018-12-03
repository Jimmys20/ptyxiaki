using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Theses
{
    public class EditModel : PageModel
    {
        private readonly ptyxiaki.Data.DepartmentContext _context;

        public EditModel(ptyxiaki.Data.DepartmentContext context)
        {
            _context = context;
        }

        [BindProperty]
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
           ViewData["professorId"] = new SelectList(_context.professors, "professorId", "professorId");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Thesis).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThesisExists(Thesis.thesisId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ThesisExists(int id)
        {
            return _context.theses.Any(e => e.thesisId == id);
        }
    }
}
