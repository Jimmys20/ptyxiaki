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
      CreateMap<Thesis, ThesisVmProfessor>().ReverseMap();
      CreateMap<Thesis, ThesisVmAdministrator>().ReverseMap();
      CreateMap<Thesis, ThesisExport>()
        .ForMember(dest => dest.professorFullName, opt => opt.MapFrom(src => src.professor != null ? src.professor.fullName : string.Empty))
        .ForMember(dest => dest.semesterTitle, opt => opt.MapFrom(src => src.semester != null ? src.semester.title : string.Empty))
        .ForMember(dest => dest.assignments, opt => opt.MapFrom(src => src.assignments != null ? string.Join(';', src.assignments.Select(a => a.student != null ? a.student.registrationNumberAndFullName : string.Empty)) : string.Empty));
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
