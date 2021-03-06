﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ApplicationCore;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;
using Web.Models;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = RolesStatic.Admin)]
    public class CustomDisplayController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IDisplayMediaService _displayMediaService;
        private readonly IAsyncRepository<DisplayMessage> _repositoryMessage;
        private readonly IMapper _mapper;

        public CustomDisplayController(IMediaService mediaService,
            IDisplayMediaService displayMediaService,
            IAsyncRepository<DisplayMessage> repositoryMessages,
            IMapper mapper)
        {
            _mediaService = mediaService;
            _displayMediaService = displayMediaService;
            _repositoryMessage = repositoryMessages;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var mediaList = await _mediaService.GetAllMediaNotUsedAsync();
            var displayMedia = await _displayMediaService.ListAllAsync();
            var messageList = await _repositoryMessage.ListAllAsync();

            ViewData["listMedia"] = _mapper.Map<IEnumerable<MediaViewModel>>(mediaList);
            ViewData["listDisplayMedia"] = _mapper.Map<IEnumerable<DisplayMediaViewModel>>(displayMedia);
            ViewData["listDisplayMessage"] = _mapper.Map<IEnumerable<DisplayMessageViewModel>>(messageList);

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

        [HttpPost]
        public async Task<IActionResult> AddNewMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
                return NotFound();

            var displayMessage = new DisplayMessage {
                Message = message,
                CreationDate = DateTime.Now
            };

            await _repositoryMessage.AddAsync(displayMessage);

            return Ok(displayMessage);
        }

        [HttpPost]
        public async Task<IActionResult> EditMessage(int? id, string message)
        {
            if (!id.HasValue || string.IsNullOrEmpty(message))
                return NotFound();

            var messageToEdit = await _repositoryMessage.GetByIdAsync(id.Value);
            messageToEdit.Message = message;
            await _repositoryMessage.UpdateAsync(messageToEdit);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeletMessage(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var messageToDelet = await _repositoryMessage.GetByIdAsync(id.Value);
            await _repositoryMessage.DeleteAsync(messageToDelet);

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveDisplayStyle([FromServices]IWritableOptions<DisplayCustom> _options, 
            DisplayCustom displayStyle)
        {
            _options.Update(opt => {
                opt.ColorFuentePrimario = displayStyle.ColorFuentePrimario;
                opt.ColorFuenteSecundario = displayStyle.ColorFuenteSecundario;
                opt.ColorPrimario = displayStyle.ColorPrimario;
                opt.ColorSecundario = displayStyle.ColorSecundario;
                opt.FontFamily = displayStyle.FontFamily;
                opt.DuracionMensajes = displayStyle.DuracionMensajes;
                opt.DuracionImagen = displayStyle.DuracionImagen;
                opt.VolumenMultimedia = displayStyle.VolumenMultimedia;
                opt.VolumenVoz = displayStyle.VolumenVoz;
            });

            return Ok();
        }
    }
}