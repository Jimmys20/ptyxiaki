using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Category
  {
    public int categoryId { get; set; }
    [Required]
    [Display(Name = "Τίτλος")]
    public string title { get; set; }
    [Display(Name = "Περιγραφή")]
    public string description { get; set; }
  }
}
