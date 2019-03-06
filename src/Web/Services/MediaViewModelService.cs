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
    public class MediaViewModelService : IMediaViewModel
    {
        private readonly IRepository<Media> _repository;
        private readonly IAppLogger<MediaViewModelService> _logger;
        private readonly IMapper _mapper;

        public MediaViewModelService(IRepository<Media> repository,
            IAppLogger<MediaViewModelService> logger,
            IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        public MediaIndexViewModel GetMediasPagination(int pageIndex, int itemsPage)
        {
            var filterPaginatedSpecification =
                new MediasFilterPaginatedSpecification(itemsPage * (pageIndex - 1), itemsPage);

            var mediasOnPage = _mapper.Map<IEnumerable<Media>, IEnumerable<MediaViewModel>>(
                _repository.List(filterPaginatedSpecification));

            var totalMedias = _repository.Count(new MediaSpecification());

            var vm = new MediaIndexViewModel
            {
                MediaList = mediasOnPage,
                PaginationInfo = new PaginationInfoViewModel
                {
                    ActualPage = pageIndex,
                    ItemsPerPage = mediasOnPage.Count(),
                    TotalItems = totalMedias,
                    TotalPages = int.Parse(Math.Ceiling((decimal)totalMedias / itemsPage).ToString())
                }
            };

            vm.PaginationInfo.Controller = "Medias";
            vm.PaginationInfo.Action = "Table";
            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages) ? "disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 1) ? "disabled" : "";

            return vm;
        }
    }
}
