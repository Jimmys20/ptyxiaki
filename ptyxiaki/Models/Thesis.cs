using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Thesis
  {
    public int thesisId { get; set; }
    public string title { get; set; }
    public string description { get; set; }

    public int professorId { get; set; }
    public Professor professor { get; set; }
  }
}
