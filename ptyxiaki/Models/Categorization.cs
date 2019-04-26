using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class Categorization
  {
    public int categorizationId { get; set; }

    public int thesisId { get; set; }
    public Thesis thesis { get; set; }

    public int categoryId { get; set; }
    public Category category { get; set; }
  }
}
