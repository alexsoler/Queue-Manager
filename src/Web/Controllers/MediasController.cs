using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;

namespace Web.Controllers
{
    public class MediasController : Controller
    {
        private readonly IMediaService _mediaService;
        private readonly IMediaViewModel _mediaViewModelService;
        private readonly IAppLogger<MediasController> _logger;

        public MediasController(IMediaService mediaService,
            IMediaViewModel mediaViewModelService,
            IAppLogger<MediasController> logger)
        {
            _mediaService = mediaService;
            _mediaViewModelService = mediaViewModelService;
            _logger = logger;
        }

        // GET: Medias
        public ActionResult Index()
        {
            return View();
        }

        [ActionName("Table")]
        public PartialViewResult PartialViewTable(int? pag)
        {
            var itemsPage = 5;
            var mediaIndexModel = _mediaViewModelService.GetMediasPagination(pag ?? 1, itemsPage);

            return PartialView("_TablaMultimedia", mediaIndexModel);
        }

        // GET: Medias/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Medias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Medias/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Medias/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Medias/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Medias/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Medias/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}