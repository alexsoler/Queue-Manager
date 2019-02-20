using ApplicationCore.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ViewModels;

namespace Web.Profiles
{
    public class AppMapperProfile : Profile
    {
        public AppMapperProfile()
        {
            CreateMap<OfficeViewModel, Office>()
                .ForMember(dest =>
                    dest.Prefix,
                    opt => opt.MapFrom(src => src.Prefix.ToString()));

            CreateMap<TaskEntity, TaskCheckboxViewModel>()
                .ForMember(dest =>
                    dest.Name,
                    opt => opt.MapFrom(src => src.Name.ToString()));

            CreateMap<ApplicationUser, OperatorCheckBoxViewModel>()
                .ForMember(dest =>
                    dest.Name,
                    opt => opt.MapFrom(src => src.Name.ToString()));

            CreateMap<TaskViewModel, TaskEntity>()
                .ForMember(dest =>
                    dest.Activo,
                    opt => opt.MapFrom(x => true));

        }
    }
}
