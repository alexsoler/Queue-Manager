﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class MediasController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IMediaViewModel _mediaViewModelService;
        private readonly IMapper _mapper;
        private readonly IAppLogger<MediasController> _logger;

        public MediasController(IMediaService mediaService,
            IMediaViewModel mediaViewModelService,
            IAppLogger<MediasController> logger,
            IMapper mapper)
        {
            _mediaService = mediaService;
            _mediaViewModelService = mediaViewModelService;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: Medias
        public ActionResult Index()
        {
            return View();
        }

        // GET: Medias/Table?pag=5
        [ActionName("Table")]
        public PartialViewResult PartialViewTable(int? pag)
        {
            var itemsPage = 5;
            var mediaIndexModel = _mediaViewModelService.GetMediasPagination(pag ?? 1, itemsPage);

            return PartialView("_TableAndPagination", mediaIndexModel);
        }

        // GET: Medias/GetMedia
        public async Task<IActionResult> GetMedia(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var file = await _mediaService.GetMediaAsync(id.Value);

            return File(file.File, file.ContentType);
        }

        // GET: Medias/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!id.HasValue)
                return Conflict("No se recibio el id");


            var media = await _mediaService.GetMediaAsync(id.Value);

            if (media == null)
                return NotFound();

            var mediavm = _mapper.Map<MediaViewModel>(media);

            return PartialView("_EliminarMedia", mediavm);
        }

        // POST: Medias/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteMedia(int? id)
        {
            if (!id.HasValue)
                return Conflict("No se recibio el id");

            try
            {
                // TODO: Add delete logic here
                var result = await _mediaService.RemoveAsync(id.Value);

                _logger.LogInformation($"Se elimino un archivo de id {id.Value}");
                return Ok(result);
            }
            catch
            {
                _logger.LogInformation($"No se pudo eliminar el archivo de id {id.Value}");
                return Conflict("No se pudo elimimar el archivo");
            }
        }

        [HttpPost]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<IActionResult> UploadFiles(IList<IFormFile> files)
        {
            _logger.LogInformation($"Se almacenaran {files.Count} archivos multimedia.");

            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var media = _mapper.Map<Media>(formFile);

                    await _mediaService.AddMediaAsync(media);
                }
            }

            _logger.LogInformation($"Se alamacenaron {files.Count} archivos multimedia. Tamaño: {size} ");

            return Ok(new { count = files.Count, size });
        }
    }
}