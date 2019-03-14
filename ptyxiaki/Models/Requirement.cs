using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Requirement
  {
    public int requirementId { get; set; }

    public int thesisId { get; set; }
    public Thesis thesis { get; set; }

    public int courseId { get; set; }
    public Course course { get; set; }
  }
}
