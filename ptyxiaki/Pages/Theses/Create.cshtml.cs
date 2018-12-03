using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ptyxiaki.Data;
using ptyxiaki.Models;

namespace ptyxiaki.Pages.Theses
{
    public class CreateModel : PageModel
    {
        private readonly ptyxiaki.Data.DepartmentContext _context;

        public CreateModel(ptyxiaki.Data.DepartmentContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["professorId"] = new SelectList(_context.professors, "professorId", "professorId");
            return Page();
        }

        [BindProperty]
        public Thesis Thesis { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.theses.Add(Thesis);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}