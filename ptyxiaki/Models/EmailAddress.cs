using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class EmailAddress
  {
    public string name { get; set; }
    public string address { get; set; }

    public EmailAddress()
    {
    }

    public EmailAddress(string name, string address)
    {
      this.name = name;
      this.address = address;
    }
  }
}
