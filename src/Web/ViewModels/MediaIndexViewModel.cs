using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class MediaIndexViewModel
    {
        public IEnumerable<MediaViewModel> MediaList { get; set; }
        public PaginationInfoViewModel PaginationInfo { get; set; }
    }
}
