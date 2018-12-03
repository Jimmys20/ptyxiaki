using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Professor
  {
    public int professorId { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string phone { get; set; }
    public string fax { get; set; }
    public string email { get; set; }
    public string place { get; set; }
    public string time { get; set; }

    public ICollection<Thesis> theses { get; set; }
  }
}
