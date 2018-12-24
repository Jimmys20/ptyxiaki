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
    }
  }
}
