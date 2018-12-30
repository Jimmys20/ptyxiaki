using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptyxiaki.Models
{
  public class MappingProfile : Profile
  {
    public MappingProfile()
    {
      CreateMap<Professor, ProfessorVm>().ReverseMap();
      CreateMap<Thesis, ThesisVm>().ReverseMap();

      CreateMap<ThesisCategory, int>().ConvertUsing(f => f.categoryId);
      CreateMap<int, ThesisCategory>().ConvertUsing(f => new ThesisCategory { categoryId = f });
      CreateMap<ThesisCourse, int>().ConvertUsing(f => f.courseId);
      CreateMap<int, ThesisCourse>().ConvertUsing(f => new ThesisCourse { courseId = f });
      CreateMap<Assignment, int>().ConvertUsing(f => f.studentId);
      CreateMap<int, Assignment>().ConvertUsing(f => new Assignment { studentId = f });
    }
  }
}
