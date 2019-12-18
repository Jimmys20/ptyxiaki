using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Course
  {
    public int courseId { get; set; }
    [Name("Κωδ.")]
    [Display(Name = "Κωδικός")]
    public string code { get; set; }
    [Name("Τίτλος")]
    [Display(Name = "Τίτλος")]
    public string title { get; set; }
    [Name("Εξάμηνο")]
    [Display(Name = "Εξάμηνο")]
    public string semester { get; set; }
  }
}
