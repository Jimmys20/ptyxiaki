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
      CreateMap<Student, StudentVm>().ReverseMap();

      CreateMap<ThesisCategory, int>().ConvertUsing(o => o.categoryId);
      CreateMap<int, ThesisCategory>().ConvertUsing(o => new ThesisCategory { categoryId = o });
      CreateMap<ThesisCourse, int>().ConvertUsing(o => o.courseId);
      CreateMap<int, ThesisCourse>().ConvertUsing(o => new ThesisCourse { courseId = o });
      CreateMap<Assignment, int>().ConvertUsing(o => o.studentId);
      CreateMap<int, Assignment>().ConvertUsing(o => new Assignment { studentId = o });
    }
  }
}
