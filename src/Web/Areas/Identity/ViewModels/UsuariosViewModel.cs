using Microsoft.QueueManager.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Areas.Identity.ViewModels
{
    public class UsuariosViewModel
    {
        public IList<ApplicationUser> Usuarios { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
