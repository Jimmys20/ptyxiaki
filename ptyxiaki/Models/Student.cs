using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Student
  {
    public int studentId { get; set; }
    [Display(Name = "Όνομα")]
    public string firstName { get; set; }
    [Display(Name = "Επώνυμο")]
    public string lastName { get; set; }
    [Display(Name = "Αριθμός Μητρώου")]
    public string registrationNumber { get; set; }
    [Display(Name = "Email")]
    public string email { get; set; }
    [Display(Name = "Εξάμηνο")]
    public string semester { get; set; }
    [Display(Name = "Γενικός Μέσος 'Ορος")]
    public string average { get; set; }
    [Display(Name = "Διδακτικές Μονάδες")]
    public string credits { get; set; }
    [Display(Name = "Συντελεστής Προόδου")]
    public string progressFactor { get; set; }

    public ICollection<Assignment> assignments { get; set; }

    public string fullName => $"{lastName} {firstName}";
  }
}
