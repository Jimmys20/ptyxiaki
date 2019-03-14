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

      CreateMap<Categorization, int>().ConvertUsing(o => o.categoryId);
      CreateMap<int, Categorization>().ConvertUsing(o => new Categorization { categoryId = o });
      CreateMap<Requirement, int>().ConvertUsing(o => o.courseId);
      CreateMap<int, Requirement>().ConvertUsing(o => new Requirement { courseId = o });
      CreateMap<Assignment, int>().ConvertUsing(o => o.studentId);
      CreateMap<int, Assignment>().ConvertUsing(o => new Assignment { studentId = o });
    }
  }
}
