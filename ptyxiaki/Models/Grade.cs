using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Grade
  {
    public int gradeId { get; set; }

    public int studentId { get; set; }
    public Student student { get; set; }

    public int courseId { get; set; }
    public Course course { get; set; }
  }
}
