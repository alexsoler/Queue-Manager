using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Controllers
{
    public class CommentsController : Controller
    {
        private readonly IAsyncRepository<Comment> _commentsRepository;
        private readonly ICommentViewModel _commentvmService;
        private readonly IMapper _mapper;

        public CommentsController(IAsyncRepository<Comment> commentsRepository,
            ICommentViewModel commentvmService,
            IMapper mapper)
        {
            _commentsRepository = commentsRepository;
            _commentvmService = commentvmService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ActionName("Table")]
        public PartialViewResult PartialViewTable(int? pag, bool isView)
        {
            var itemsPage = 5;
            var commentIndexModel =  _commentvmService.GetCommentsPagination(pag ?? 1, itemsPage, isView).Result;

            return PartialView("_TableAndPagination", commentIndexModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveComment(CommentViewModel commentViewModel)
        {
            var comment = _mapper.Map<Comment>(commentViewModel);

            comment.CreationDate = DateTime.Now;

            await _commentsRepository.AddAsync(comment);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeCommentToView(int? id)
        {
            if (!id.HasValue) return NotFound();

            var comment = await _commentsRepository.GetByIdAsync(id.Value);

            if (comment is null) return NotFound();

            comment.IsView = true;

            await _commentsRepository.UpdateAsync(comment);

            return Ok();
        }
    }
}