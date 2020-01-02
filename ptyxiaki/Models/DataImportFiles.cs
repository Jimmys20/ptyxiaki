using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class DataImportFiles
  {
    [Display(Name = "Φοιτητές")]
    public IFormFile students { get; set; }
    [Display(Name = "Μαθήματα")]
    public IFormFile courses { get; set; }
    [Display(Name = "Βαθμολογίες")]
    public IFormFile grades { get; set; }
  }
}
