using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Semester
  {
    public int semesterId { get; set; }
    [Required]
    [Display(Name = "Τίτλος")]
    public string title { get; set; }
    [DataType(DataType.Date)]
    [Display(Name = "Δημιουργήθηκε")]
    public DateTime createdAt { get; set; }
  }
}
