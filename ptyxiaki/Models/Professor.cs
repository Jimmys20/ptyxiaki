using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Professor
  {
    public int professorId { get; set; }
    public string oAuthId { get; set; }
    public bool isAdmin { get; set; }
    [Display(Name = "Όνομα")]
    public string firstName { get; set; }
    [Display(Name = "Επώνυμο")]
    public string lastName { get; set; }
    [Display(Name = "Γραφείο")]
    public string office { get; set; }
    [Display(Name = "Ώρες συνεργασίας")]
    public string time { get; set; }
    [DataType(DataType.PhoneNumber)]
    [Display(Name = "Τηλέφωνο")]
    public string phone { get; set; }
    [Display(Name = "Fax")]
    public string fax { get; set; }
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    [Display(Name = "Email")]
    public string email { get; set; }
    [DataType(DataType.Url)]
    [Display(Name = "Ιστοσελίδα")]
    public string website { get; set; }

    public List<Thesis> theses { get; set; }

    [Display(Name = "Ονοματεπώνυμο")]
    public string fullName => $"{lastName} {firstName}";
  }

  [ModelMetadataType(typeof(Professor))]
  public class ProfessorVm
  {
    public string office { get; set; }
    public string time { get; set; }
    public string phone { get; set; }
    public string fax { get; set; }
    public string email { get; set; }
    public string website { get; set; }
  }
}
