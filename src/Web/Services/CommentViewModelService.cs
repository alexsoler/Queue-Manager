using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using ApplicationCore.Specifications;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Interfaces;
using Web.ViewModels;

namespace Web.Services
{
    public class CommentViewModelService : ICommentViewModel
    {
        private readonly IAsyncRepository<Comment> _asyncRepository;
        private readonly IMapper _mapper;

        public CommentViewModelService(IAsyncRepository<Comment> asyncRepository,
            IMapper mapper)
        {
            _asyncRepository = asyncRepository;
            _mapper = mapper;
        }
        public async Task<CommentIndexViewModel> GetCommentsPagination(int pageIndex, int itemsPage, bool isView)
        {
            var filterPaginatedSpecification =
                new CommentsFilterPaginatedSpecification(itemsPage * (pageIndex - 1), itemsPage, isView);

            var commentsOnPage = _mapper.Map<IEnumerable<Comment>, IEnumerable<CommentViewModel>>(
                await _asyncRepository.ListAsync(filterPaginatedSpecification));

            var totalComments = await _asyncRepository.CountAsync(new CommentSpecification(isView));

            var vm = new CommentIndexViewModel
            {
                CommentsList = commentsOnPage,
                PaginationInfo = new PaginationInfoViewModel
                {
                    ActualPage = pageIndex,
                    ItemsPerPage = commentsOnPage.Count(),
                    TotalItems = totalComments,
                    TotalPages = int.Parse(Math.Ceiling((decimal)totalComments / itemsPage).ToString())
                }
            };

            vm.PaginationInfo.Controller = "Comments";
            vm.PaginationInfo.Action = "Table";
            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages) ? "disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 1) ? "disabled" : "";

            return vm;
        }
    }
}
