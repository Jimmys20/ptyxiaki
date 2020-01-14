using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Thesis
  {
    public int thesisId { get; set; }
    [Required]
    [Display(Name = "Τίτλος")]
    public string title { get; set; }
    [Required]
    [Display(Name = "Τίτλος (Αγγλικά)")]
    public string englishTitle { get; set; }
    [Required]
    [Display(Name = "Περιγραφή")]
    public string description { get; set; }
    [Display(Name = "Βιβλιογραφία")]
    public string bibliography { get; set; }
    [Display(Name = "Πηγές")]
    public string sources { get; set; }
    [Display(Name = "Εργαλεία")]
    public string tools { get; set; }
    [Display(Name = "Περιορισμοί")]
    public string restrictions { get; set; }

    //------------------------------------------

    [Required]
    [MinLength(2)]
    [MaxLength(4)]
    [Display(Name = "Προαπαιτούμενα μαθήματα")]
    public List<Requirement> requirements { get; set; }
    [Required]
    [Display(Name = "Κατηγορία Δ/Ε")]
    public List<Categorization> categorizations { get; set; }

    //------------------------------------------

    [Display(Name = "Τύπος ανάθεσης")]
    public AssignmentType assignmentType { get; set; }
    [Display(Name = "Κατάσταση")]
    public Status status { get; set; }

    public int semesterId { get; set; }
    [Display(Name = "Εξάμηνο")]
    public Semester semester { get; set; }

    [Display(Name = "Επιβλέπων καθηγητής")]
    public int professorId { get; set; }
    [Display(Name = "Επιβλέπων καθηγητής")]
    public Professor professor { get; set; }

    //------------------------------------------

    [DataType(DataType.Text)]
    [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Ημερομηνία δημιουργίας")]
    public DateTime? createdAt { get; set; }

    [DataType(DataType.Text)]
    [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Ημερομηνία ακύρωσης")]
    public DateTime? canceledAt { get; set; }

    [DataType(DataType.Text)]
    [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Ημερομηνία ανάθεσης")]
    public DateTime? assignedAt { get; set; }

    [DataType(DataType.Text)]
    [DisplayFormat(DataFormatString = "{0:d/M/yyyy}", ApplyFormatInEditMode = true)]
    [Display(Name = "Ημερομηνία ολοκλήρωσης")]
    public DateTime? completedAt { get; set; }

    [Display(Name = "Λόγος ακύρωσης")]
    public string cancelReason { get; set; }

    //------------------------------------------

    [Display(Name = "Φοιτητής")]
    public List<Assignment> assignments { get; set; }

    [Display(Name = "Δηλώσεις")]
    public List<Declaration> declarations { get; set; }

    public string filePath { get; set; }
  }

  [ModelMetadataType(typeof(Thesis))]
  public class ThesisVmProfessor : IValidatableObject
  {
    public string title { get; set; }
    public string englishTitle { get; set; }
    public string description { get; set; }
    public string bibliography { get; set; }
    public string sources { get; set; }
    public string tools { get; set; }
    public string restrictions { get; set; }
    public AssignmentType assignmentType { get; set; }
    public List<int> assignments { get; set; }
    public List<int> requirements { get; set; }
    public List<int> categorizations { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
      if (assignmentType == AssignmentType.Individual && assignments != null && assignments.Count != 1)
      {
        yield return new ValidationResult("Πρέπει να οριστεί ένας φοιτητής όταν ο τύπος ανάθεσης είναι \"Ατομική\".", new[] { "assignments" });
      }
      else if (assignmentType == AssignmentType.Group && assignments != null && assignments.Count != 2)
      {
        yield return new ValidationResult("Πρέπει να οριστούν δύο φοιτητές όταν ο τύπος ανάθεσης είναι \"Ομαδική\".", new[] { "assignments" });
      }
    }
  }

  [ModelMetadataType(typeof(Thesis))]
  public class ThesisVmAdministrator
  {
    public int professorId { get; set; }
    public Status status { get; set; }
    public DateTime? createdAt { get; set; }
    public DateTime? assignedAt { get; set; }
    public DateTime? canceledAt { get; set; }
    public DateTime? completedAt { get; set; }
  }

  [JsonConverter(typeof(StringEnumConverter))]
  public enum AssignmentType
  {
    [EnumMember(Value = "Ατομική")]
    [Display(Name = "Ατομική")]
    Individual,
    [EnumMember(Value = "Ομαδική")]
    [Display(Name = "Ομαδική")]
    Group
  }

  public enum Status
  {
    [Display(Name = "Διαθέσιμη")]
    Available,
    [Display(Name = "Μη διαθέσιμη")]
    Unavailable,
    [Display(Name = "Ενεργή")]
    Active,
    [Display(Name = "Ακυρωμένη")]
    Canceled,
    [Display(Name = "Ολοκληρωμένη")]
    Completed
  }

  public class ThesisExport
  {
    public int thesisId { get; set; }
    public string title { get; set; }
    public string englishTitle { get; set; }
    public AssignmentType assignmentType { get; set; }
    public Status status { get; set; }
    public DateTime? createdAt { get; set; }
    public DateTime? canceledAt { get; set; }
    public DateTime? assignedAt { get; set; }
    public DateTime? completedAt { get; set; }
    public string cancelReason { get; set; }
    public string semesterTitle { get; set; }
    public string professorFullName { get; set; }
    public string assignments { get; set; }
  }
}
