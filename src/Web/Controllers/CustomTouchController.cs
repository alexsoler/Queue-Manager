using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Interfaces;
using ApplicationCore.Services;
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
    public class CustomTouchController : Controller
    {
        private readonly IWritableOptions<TouchCustom> _optionsTouch;
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;

        public CustomTouchController(IWritableOptions<TouchCustom> optionsTouch,
            IMediaService mediaService,
            IMapper mapper)
        {
            _optionsTouch = optionsTouch;
            _mediaService = mediaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["listImages"] = new SelectList(
                _mapper.Map<IEnumerable<MediaViewModel>>(await _mediaService.GetImages()),
                "Url", "Name");
            return View(_optionsTouch.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveTouchCustom(TouchCustom touchCustom)
        {
            _optionsTouch.Update(opt =>
            {
                opt.BackgroundColor = touchCustom.BackgroundColor;
                opt.BackgroundImage = touchCustom.BackgroundImage;
                opt.ColorButtonPriority = touchCustom.ColorButtonPriority;
                opt.ColorButtonTask = touchCustom.ColorButtonTask;
                opt.FontFamily = touchCustom.FontFamily;
                opt.MensajeNotificacion = touchCustom.MensajeNotificacion;
                opt.PathImageLogo = touchCustom.PathImageLogo;
                opt.ShowBackgroundColor = touchCustom.ShowBackgroundColor;
                opt.ShowLogo = touchCustom.ShowLogo;
                opt.ShowTitle = touchCustom.ShowTitle;
                opt.Title = touchCustom.Title;
            });

            return Ok();
        }
    }
}