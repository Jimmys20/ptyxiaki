using CsvHelper.Configuration.Attributes;
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
    [Name("ΟΝΟΜΑ")]
    [Display(Name = "Όνομα")]
    public string firstName { get; set; }
    [Name("ΕΠΩΝΥΜΟ")]
    [Display(Name = "Επώνυμο")]
    public string lastName { get; set; }
    [Name("ΠΑΤΡΩΝΥΜΟ")]
    [Display(Name = "Πατρώνυμο")]
    public string fatherName { get; set; }
    [Name("Α.Μ.")]
    [Display(Name = "Αριθμός μητρώου")]
    public int registrationNumber { get; set; }
    [Name("ΕΞΑΜΗΝΟ")]
    [Display(Name = "Εξάμηνο")]
    public int semester { get; set; }
    [Name("ΒΑΘΜΟΣ")]
    [Display(Name = "Γενικός μέσος όρος")]
    public float average { get; set; }
    [Name("ΠΕΡΑΣΜΕΝΣ Δ.Μ.")]
    [Display(Name = "Διδακτικές μονάδες")]
    public int credits { get; set; }
    [Name("ΣΥΝΤΕΛΕΣΤΗΣ ΠΡΟΟΔΟΥ")]
    [Display(Name = "Συντελεστής προόδου")]
    public int progressFactor { get; set; }

    public ICollection<Assignment> assignments { get; set; }

    [Display(Name = "Ονοματεπώνυμο")]
    public string fullName => $"{lastName} {firstName}";
    [Display(Name = "Email")]
    public string email => $"it{registrationNumber}@it.teithe.gr";
  }
}
