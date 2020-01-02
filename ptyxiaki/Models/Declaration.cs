using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Declaration
  {
    public int declarationId { get; set; }

    public int studentId { get; set; }
    public Student student { get; set; }

    public int thesisId { get; set; }
    public Thesis thesis { get; set; }

    public int semesterId { get; set; }
    public Semester semester { get; set; }
  }
}
