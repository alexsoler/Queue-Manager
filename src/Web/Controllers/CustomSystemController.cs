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
    public class CustomSystemController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;
        private readonly IWritableOptions<SystemCustom> _options;

        public CustomSystemController(IMapper mapper,
            IMediaService mediaService,
            IWritableOptions<SystemCustom> options)
        {
            _mapper = mapper;
            _mediaService = mediaService;
            _options = options;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["listImages"] = new SelectList(
                _mapper.Map<IEnumerable<MediaViewModel>>(await _mediaService.GetImages()),
                "Url", "Name");
            return View(_options.Value);
        }

        public IActionResult SaveSystemCustom(SystemCustom systemCustom)
        {
            _options.Update(opt =>
            {
                opt.Logo = systemCustom.Logo;
                opt.LogoMin = systemCustom.LogoMin;
                opt.Nombre = systemCustom.Nombre;
                opt.Telefono = systemCustom.Telefono;
                opt.Email = systemCustom.Email;
                opt.Descripcion = systemCustom.Descripcion;
            });

            return Ok();
        }
    }
}