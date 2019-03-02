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
            CreateMap<OfficeViewModel, Office>();

            CreateMap<Office, OfficeViewModel>();

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

            CreateMap<Media, MediaViewModel>()
                .ForMember(dest =>
                    dest.Tipo,
                    opt => opt.MapFrom((media, mediavm) =>
                    {
                        if (media.Img)
                            return "Imagen";
                        else if (media.Video)
                            return "Video";
                        else if (media.Audio)
                            return "Audio";
                        return string.Empty;
                    }))
                .ForMember(dest =>
                    dest.File,
                    opt => opt.MapFrom(x => default(byte[])));

        }
    }
}
