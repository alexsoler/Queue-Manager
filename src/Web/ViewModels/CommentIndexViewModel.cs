using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.ViewModels
{
    public class CommentIndexViewModel
    {
        public IEnumerable<CommentViewModel> CommentsList { get; set; }
        public PaginationInfoViewModel PaginationInfo { get; set; }
    }
}
