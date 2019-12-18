using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Grade
  {
    public int gradeId { get; set; }
    [Name("student_ID")]
    public int student_ID { get; set; }
    [Name("spec_aem")]
    public string spec_aem { get; set; }
    [Name("courseID")]
    public string courseID { get; set; }
    [Name("coursecode")]
    public string coursecode { get; set; }
    [Display(Name = "Μάθημα")]
    [Name("title")]
    public string title { get; set; }
    [Display(Name = "Έτος")]
    [Name("cyear")]
    public int cyear { get; set; }
    [Name("cperiod")]
    public int cperiod { get; set; }
    [Name("cregtypeID")]
    public int cregtypeID { get; set; }
    [Name("epshort")]
    public string epshort { get; set; }
    [Display(Name = "Βαθμός")]
    [Name("cgrade")]
    public float cgrade { get; set; }
    [Name("notes")]
    public string notes { get; set; }
  }
}
