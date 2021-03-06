﻿using ApplicationCore.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Web.Hubs.ParametersObject;
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
                    }));

            CreateMap<DisplayMedia, DisplayMediaViewModel>()
                .ForMember(dest =>
                    dest.Tipo,
                    opt => opt.MapFrom((displayMedia, displayMediavm) =>
                    {
                        if (displayMedia.Media.Img)
                            return "Imagen";
                        else if (displayMedia.Media.Video)
                            return "Video";
                        else if (displayMedia.Media.Audio)
                            return "Audio";
                        return string.Empty;
                    }))
                .ForMember(dest =>
                    dest.ContentType,
                    opt => opt.MapFrom(x => x.Media.ContentType))
               .ForMember(dest =>
                    dest.Nombre,
                    opt => opt.MapFrom(x => x.Media.Name))
               .ForMember(dest =>
                    dest.IdMedia,
                    opt => opt.MapFrom(x => x.Media.Id))
               .ForMember(dest =>
                    dest.Id,
                    opt => opt.MapFrom(x => x.Id));

            CreateMap<IFormFile, Media>()
                .ForMember(dest =>
                    dest.Name,
                    opt => opt.MapFrom(src => src.FileName))
                .ForMember(dest =>
                    dest.CreationDate,
                    opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest =>
                    dest.Img,
                    opt => opt.MapFrom(src => src.ContentType.Contains("image")))
                .ForMember(dest =>
                    dest.Video,
                    opt => opt.MapFrom(src => src.ContentType.Contains("video")))
                .ForMember(dest =>
                    dest.Audio,
                    opt => opt.MapFrom(src => src.ContentType.Contains("audio")));

            CreateMap<Ticket, TicketParameter>()
                .ForMember(dest =>
                    dest.NamePriority,
                    opt => opt.MapFrom(x => x.Priority.Name))
                .ForMember(dest =>
                    dest.NameTask,
                    opt => opt.MapFrom(x => x.TaskEntity.Name))
                .ForMember(dest =>
                    dest.CreationDate,
                    opt => opt.MapFrom(x => x.CreationDate.TimeOfDay.ToString(@"hh\:mm\:ss")));

            CreateMap<Ticket, TicketDisplayParameter>()
                .ForMember(dest =>
                    dest.Ticket,
                    opt => opt.MapFrom(x => x.DisplayTokenName))
                .ForMember(dest =>
                    dest.Office,
                    opt => opt.MapFrom(x => x.Office.Name));

            CreateMap<Ticket, TicketReportViewModel>()
                .ForMember(dest =>
                    dest.NamePriority,
                    opt => opt.MapFrom(x => x.Priority.Name))
                .ForMember(dest =>
                    dest.NameTask,
                    opt => opt.MapFrom(x => x.TaskEntity.Name))
                .ForMember(dest =>
                    dest.CreationDate,
                    opt => opt.MapFrom(x => x.CreationDate.ToString("dd/MM/yyyy hh:mm tt")))
                .ForMember(dest =>
                    dest.Estado,
                    opt => opt.MapFrom(x => x.Status.Name))
                .ForMember(dest =>
                    dest.Duracion,
                    opt => opt.MapFrom((ticket, ticketReport) =>
                    {
                        if(ticket.CompletionAttentionDate.HasValue)
                        {
                            return (ticket.CompletionAttentionDate - ticket.CreationDate).Value.ToString(@"mm\m\ ss\s");
                        }

                        return "0m";
                    }));

            CreateMap<DisplayMessage, DisplayMessageViewModel>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<CommentViewModel, Comment>();
        }
    }
}
