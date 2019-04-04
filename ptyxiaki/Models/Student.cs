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
    [Display(Name = "Όνομα")]
    public string firstName { get; set; }
    [Display(Name = "Επώνυμο")]
    public string lastName { get; set; }
    [Display(Name = "Πατρώνυμο")]
    public string fatherName { get; set; }
    [Display(Name = "Αριθμός μητρώου")]
    public string registrationNumber { get; set; }
    [Display(Name = "Εξάμηνο")]
    public int semester { get; set; }
    [Display(Name = "Γενικός μέσος όρος")]
    public float average { get; set; }
    [Display(Name = "Διδακτικές μονάδες")]
    public int credits { get; set; }
    [Display(Name = "Συντελεστής προόδου")]
    public int progressFactor { get; set; }

    [Display(Name = "Πτυχιακές εργασίες")]
    public List<Assignment> assignments { get; set; }
    public List<Declaration> declarations { get; set; }
    public List<Grade> grades { get; set; }

    [Display(Name = "Ονοματεπώνυμο")]
    public string fullName => $"{lastName} {firstName}";
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public string email => $"it{registrationNumber}@it.teithe.gr";
    public string average_ => average.ToString("0.#");
  }

  public class StudentVm
  {
    [Name("ΟΝΟΜΑ")]
    public string firstName { get; set; }
    [Name("ΕΠΩΝΥΜΟ")]
    public string lastName { get; set; }
    [Name("ΠΑΤΡΩΝΥΜΟ")]
    public string fatherName { get; set; }
    [Name("Α.Μ.")]
    public string registrationNumber { get; set; }
    [Name("ΕΞΑΜΗΝΟ")]
    public int semester { get; set; }
    [Name("ΒΑΘΜΟΣ")]
    public float average { get; set; }
    [Name("ΠΕΡΑΣΜΕΝΣ Δ.Μ.")]
    public int credits { get; set; }
    [Name("ΣΥΝΤΕΛΕΣΤΗΣ ΠΡΟΟΔΟΥ")]
    public int progressFactor { get; set; }
  }
}
