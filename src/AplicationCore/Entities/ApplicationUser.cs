﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public bool Activo { get; set; }
        public DateTime CreationDate { get; set; }
        public List<OfficeOperator> VentanillasOperadores { get; set; }
    }
}
