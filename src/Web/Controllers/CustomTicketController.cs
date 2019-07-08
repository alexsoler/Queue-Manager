using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Interfaces;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = RolesStatic.Admin)]
    public class CustomTicketController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;
        private readonly IWritableOptions<TicketCustom> _options;

        public CustomTicketController(IWritableOptions<TicketCustom> options,
            IMediaService mediaService,
            IMapper mapper)
        {
            _mediaService = mediaService;
            _mapper = mapper;
            _options = options;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["listImages"] = new SelectList(
                _mapper.Map<IEnumerable<MediaViewModel>>(await _mediaService.GetImages()),
                "Url", "Name");
            return View(_options.Value);
        }

        public IActionResult SaveTicketCustom(TicketCustom ticketCustom)
        {
            _options.Update(opt =>
            {
                opt.FontFamily = ticketCustom.FontFamily;
                opt.FontStyleDate = ticketCustom.FontStyleDate;
                opt.FontStyleDisplayTicket = ticketCustom.FontStyleDisplayTicket;
                opt.FontStylePriority = ticketCustom.FontStylePriority;
                opt.FontStyleTask = ticketCustom.FontStyleTask;
                opt.FontStyleTelefono = ticketCustom.FontStyleTelefono;
                opt.FontStyleTitle = ticketCustom.FontStyleTitle;
                opt.FontWeightDate = ticketCustom.FontWeightDate;
                opt.FontWeightDisplayTicket = ticketCustom.FontWeightDisplayTicket;
                opt.FontWeightPriority = ticketCustom.FontWeightPriority;
                opt.FontWeightTask = ticketCustom.FontWeightTask;
                opt.FontWeightTelefono = ticketCustom.FontWeightTelefono;
                opt.FontWeightTitle = ticketCustom.FontWeightTitle;
                opt.Logo = ticketCustom.Logo;
                opt.PageHeight = ticketCustom.PageHeight;
                opt.PageWidth = ticketCustom.PageWidth;
                opt.ShowDate = ticketCustom.ShowDate;
                opt.ShowLogo = ticketCustom.ShowLogo;
                opt.ShowPriority = ticketCustom.ShowPriority;
                opt.ShowTask = ticketCustom.ShowTask;
                opt.ShowTelefono = ticketCustom.ShowTelefono;
                opt.ShowTicket = ticketCustom.ShowTicket;
                opt.ShowTitle = ticketCustom.ShowTitle;
                opt.SizeDate = ticketCustom.SizeDate;
                opt.SizeDisplayTicket = ticketCustom.SizeDisplayTicket;
                opt.SizePriority = ticketCustom.SizePriority;
                opt.SizeTask = ticketCustom.SizeTask;
                opt.SizeTelefono = ticketCustom.SizeTelefono;
                opt.SizeTitle = ticketCustom.SizeTitle;
                opt.Telefono = ticketCustom.Telefono;
                opt.Title = ticketCustom.Title;
            });

            return Ok();
        }
    }
}