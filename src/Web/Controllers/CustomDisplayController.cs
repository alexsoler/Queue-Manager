using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web.ViewModels;

namespace Web.Controllers
{
    public class CustomDisplayController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IDisplayMediaService _displayMediaService;
        private readonly IMapper _mapper;

        public CustomDisplayController(IMediaService mediaService,
            IDisplayMediaService displayMediaService,
            IMapper mapper)
        {
            _mediaService = mediaService;
            _displayMediaService = displayMediaService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var mediaList = await _mediaService.GetAllMediaNotUsedAsync();
            var displayMedia = await _displayMediaService.ListAllAsync();

            ViewData["listMedia"] = _mapper.Map<IEnumerable<MediaViewModel>>(mediaList);
            ViewData["listDisplayMedia"] = _mapper.Map<IEnumerable<DisplayMediaViewModel>>(displayMedia);

            return View();
        }

        public async Task<ActionResult> DisplayMediaPV()
        {
            var displayMedia = await _displayMediaService.ListAllAsync();
            var listDisplayMedia = _mapper.Map<IEnumerable<DisplayMediaViewModel>>(displayMedia);

            return PartialView("_listDisplay", listDisplayMedia);
        }

        public async Task<ActionResult> MediaPV()
        {
            var mediaList = await _mediaService.GetAllMediaNotUsedAsync();
            var listMedia = _mapper.Map<IEnumerable<MediaViewModel>>(mediaList);

            return PartialView("_listMedia", listMedia);
        }

        [HttpPost]
        public async Task<IActionResult> AddMediaToDisplay(int? id, double? order)
        {
            if (!id.HasValue || !order.HasValue)
                return BadRequest();

            var displayMedia = new DisplayMedia
            {
                MediaId = id.Value,
                Order = order.Value,
                CreationDate = DateTime.Now
            };

            await _displayMediaService.AddAsync(displayMedia);
            await _mediaService.SetAsUsed(id.Value);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveMediaToDisplay(int? id, int? idMedia)
        {
            if (!id.HasValue || !idMedia.HasValue)
                return BadRequest();

            await _displayMediaService.RemoveAsync(id.Value);
            await _mediaService.SetAsNotUsed(idMedia.Value);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(int? id, double? order)
        {
            if (!id.HasValue || !order.HasValue)
                return BadRequest();

            await _displayMediaService.UpdateOrder(id.Value, order.Value);

            return Ok();
        }
    }
}