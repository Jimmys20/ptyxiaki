using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Date : IValidatableObject
  {
    public int dateId { get; set; }

    [DataType(DataType.Text)]
    [DisplayFormat(DataFormatString = "{0:d/M/yyyy H:mm}", ApplyFormatInEditMode = true)]
    [Display(Name = "Έναρξη ανάρτησης θεμάτων")]
    public DateTime? postStart { get; set; }

    [DataType(DataType.Text)]
    [DisplayFormat(DataFormatString = "{0:d/M/yyyy H:mm}", ApplyFormatInEditMode = true)]
    [Display(Name = "Λήξη ανάρτησης θεμάτων")]
    public DateTime? postEnd { get; set; }

    [DataType(DataType.Text)]
    [DisplayFormat(DataFormatString = "{0:d/M/yyyy H:mm}", ApplyFormatInEditMode = true)]
    [Display(Name = "Έναρξη δηλώσεων")]
    public DateTime? declarationStart { get; set; }

    [DataType(DataType.Text)]
    [DisplayFormat(DataFormatString = "{0:d/M/yyyy H:mm}", ApplyFormatInEditMode = true)]
    [Display(Name = "Λήξη δηλώσεων")]
    public DateTime? declarationEnd { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (postEnd < postStart)
      {
        yield return new ValidationResult("Η λήξη ανάρτησης θεμάτων πρέπει να είναι μετά την έναρξη ανάρτησης θεμάτων.", new[] { "postEnd" });
      }
      if (declarationEnd < declarationStart)
      {
        yield return new ValidationResult("Η λήξη δηλώσεων πρέπει να είναι μετά την έναρξη δηλώσεων.", new[] { "declarationEnd" });
      }
    }
  }
}
