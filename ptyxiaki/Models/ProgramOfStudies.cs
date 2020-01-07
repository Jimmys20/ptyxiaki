using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class ProgramOfStudies
  {
    public int programOfStudiesId { get; set; }
    [Required]
    [Display(Name = "Τίτλος")]
    public string title { get; set; }

    [Display(Name = "Μαθήματα")]
    public List<Course> courses { get; set; }
  }
}
