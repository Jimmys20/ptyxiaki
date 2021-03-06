﻿using System;
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

namespace ptyxiaki.Pages.Administration.Categories
{
  public class IndexModel : PageModel
  {
    private readonly DepartmentContext context;

    public IndexModel(DepartmentContext context)
    {
      this.context = context;
    }

    public List<Category> categories { get; set; }

    public async Task OnGetAsync()
    {
      categories = await context.categories.ToListAsync();
    }
  }
}
