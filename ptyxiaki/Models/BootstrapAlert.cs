using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class BootstrapAlert
  {
    public string type { get; set; }
    public string message { get; set; }

    public BootstrapAlert()
    {
    }

    public BootstrapAlert(string type, string message)
    {
      this.type = type;
      this.message = message;
    }
  }
}
