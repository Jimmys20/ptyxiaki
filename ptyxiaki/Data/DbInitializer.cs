using ptyxiaki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Data
{
  public static class DbInitializer
  {
    public static void Initialize(DepartmentContext context)
    {
      context.Database.EnsureDeleted();
      context.Database.EnsureCreated();

      if (context.professors.Any())
      {
        return;
      }

      var professors = new Professor[]
      {
        new Professor{professorId=-1,firstName="takis",lastName="takis",email="" },
        new Professor{professorId=-2,firstName="lakis",lastName="lakis",email="" },
        new Professor{professorId=-3,firstName="mpampis",lastName="mpampis",email="" },
        new Professor{professorId=-4,firstName="aaa",lastName="aaa",email="" },
        new Professor{professorId=-5,firstName="bbb",lastName="bbb",email="" },
        new Professor{professorId=-6,firstName="ccc",lastName="ccc",email="" },
      };
      foreach (Professor p in professors)
      {
        context.professors.Add(p);
      }
      context.SaveChanges();

      var theses = new Thesis[]
      {
        new Thesis{ thesisId=-1, professorId=-1, title="java",description=""},
        new Thesis{ thesisId=-2, professorId=-1, title="app",description=""},
        new Thesis{ thesisId=-3, professorId=-3, title="web",description=""},
        new Thesis{ thesisId=-4, professorId=-3, title="c#",description=""},
        new Thesis{ thesisId=-5, professorId=-4, title="whatever",description=""},
        new Thesis{ thesisId=-6, professorId=-5, title="",description=""},
        new Thesis{ thesisId=-7, professorId=-5, title="",description=""}
      };
      foreach (Thesis t in theses)
      {
        context.theses.Add(t);
      }
      context.SaveChanges();
    }
  }
}
